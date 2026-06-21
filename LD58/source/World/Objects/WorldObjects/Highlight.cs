using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Graphics.OpenGl.Lights;
using static ChaosFramework.Math.Constants;

namespace LD58.World.Objects.WorldObjects
{
    internal class Highlight
        : WorldObject
    {
        SpotLight light;
        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            light = new SpotLight(
                bone.GetPosition(),
                Rgba.OPAQUE_WHITE,
                14,
                PI / 12,
                0.1f
                );
            light.direction = bone.GetDirection();
            scene.lights.Add(light);
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        { }

        protected override void DoDispose()
        {
            base.DoDispose();
            light.Dispose();
        }
    }
}
