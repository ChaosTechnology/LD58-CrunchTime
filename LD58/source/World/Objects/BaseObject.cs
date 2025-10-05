using System;
using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using ChaosUtil.Reflection;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    public abstract class BaseObject
        : Component<Stage>
        , Instancable
    {
        protected Rig.Bone bone { get; private set; }

        public void GiveMeInstances(InstancingAttribute[] instancers)
        {
            instancers[0].informer.AddInstance(bone.GetBoneTransform());
        }

        public bool NeedsInstancedDraw()
            => true;

        protected override void Create(CreateParameters args)
        {
            CParams<Rig.Bone> boneArgs = CreateParameters.RequireAs<Rig.Bone>(args);
            bone = boneArgs.v1;
            scene.instancers.GetOrCreateManager(GetType()).instancedThings.Add(this);
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            scene.instancers.GetOrCreateManager(GetType()).instancedThings.Remove(this);
        }
    }
}
