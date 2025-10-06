using ChaosFramework.Graphics.OpenGl.Instancing;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Glass Wall Base.gmdl", "objects/Apartment Floor.mat")]
    [GlassInstancer(64, "objects/Glass Wall Transparent.gmdl")]
    class GlassWall
        : WorldObject
    {
        public GlassWall() : base(1, 1) { }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            instancers[1].informer.AddInstance(bone.GetBoneTransform());
        }
    }
}
