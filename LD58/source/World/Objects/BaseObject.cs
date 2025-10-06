using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Shapes.Rigging;
using ChaosUtil.Reflection;
using System;
using System.Reflection;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    public abstract class BaseObject
        : Component<Stage>
        , Instancable
    {
        [AttributeUsage(AttributeTargets.Field)]
        protected class BoneParameter
            : Attribute
        { }

        protected Rig.Bone bone { get; private set; }

        public virtual void GiveMeInstances(InstancingAttribute[] instancers)
            => instancers[0].informer.AddInstance(bone.GetBoneTransform());

        public bool NeedsInstancedDraw()
            => true;

        protected override void Create(CreateParameters args)
        {
            CParams<Rig.Bone> boneArgs = CreateParameters.RequireAs<Rig.Bone>(args);
            bone = boneArgs.v1;
            scene.instancers.GetOrCreateManager(GetType()).instancedThings.Add(this);

            int dot = bone.name.IndexOf('.');
            string boneNameWithParams = dot < 0 ? bone.name : bone.name.Substring(0, dot);

            int paraOpen = boneNameWithParams.IndexOf('(');
            if (paraOpen >= 0)
            {
                int paraClose = boneNameWithParams.IndexOf(')', paraOpen);
                if (paraClose < 0)
                    throw new InvalidOperationException("Missing closing parantheses!");

                foreach (string assignment in boneNameWithParams.Substring(paraOpen + 1, paraClose - paraOpen - 1).Split(';'))
                {
                    string[] ass = assignment.Split(':');
                    if (ass.Length != 2)
                        throw new System.InvalidOperationException("Malformed assignment");
                    ass[0] = ass[0].Trim();
                    ass[1] = ass[1].Trim();

                    foreach (FieldInfo field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                        if (field.Name == ass[0])
                        {
                            Delegate parser;
                            if (!ChaosUtil.Serialization.Text.Parse.TryGetParser(field.FieldType, out parser))
                                throw new InvalidOperationException(
                                    $"No parser for {field.FieldType} {field.DeclaringType.Name}.{field.Name}!"
                                    );
                            else if (field.GetAttributes<BoneParameter>(true).Length > 0)
                            {
                                object[] parserArgs = { ass[1], null };
                                if ((bool)parser.DynamicInvoke(parserArgs))
                                {
                                    field.SetValue(this, parserArgs[1]);
                                    goto success;
                                }
                            }
                        }

                    throw new Exception($"Could not assign field {ass[0]}!");
                success:;
                }
            }
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            scene.instancers.GetOrCreateManager(GetType()).instancedThings.Remove(this);
        }
    }
}
