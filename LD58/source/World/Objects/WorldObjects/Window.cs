using ChaosFramework.Graphics.OpenGl.Instancing;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Window.gmdl", "objects/Apartment Wall.mat")]
    [GlassInstancer(64, "objects/Window Transparent.gmdl")]
    class Window
        : WorldObject
    {
        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            instancers[1].informer.AddInstance(bone.GetBoneTransform());
        }
    }
}
