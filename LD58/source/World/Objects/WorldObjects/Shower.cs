using ChaosFramework.Graphics.OpenGl.Instancing;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Shower.gmdl", "objects/Bathroom.mat")]
    [GlassInstancer(64, "objects/Shower Transparent.gmdl")]
    class Shower
        : WorldObject
    {
        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            instancers[1].informer.AddInstance(bone.GetBoneTransform());
        }
    }
}
