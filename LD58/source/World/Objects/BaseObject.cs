using System;
using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using ChaosUtil.Reflection;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    public abstract class BaseObject
        : Component<Stage>
    {
        protected Rig.Bone bone { get; private set; }

        protected override void Create(CreateParameters args)
        {
            CParams<Rig.Bone> boneArgs = CreateParameters.RequireAs<Rig.Bone>(args);
            bone = boneArgs.v1;
        }
    }
}
