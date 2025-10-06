using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Graphics.OpenGl.Lights;
using ChaosFramework.Math.Vectors;
using static ChaosFramework.Math.Constants;

namespace LD58.World.Objects.WorldObjects
{
    using World.Objects;

    class Highlight
        : DecoObject
    {
        SpotLight light;

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            Vector3f bonePos = bone.GetPosition();
            Vector3f boneDir = bone.GetDirection() * 1.5f;
            light = new SpotLight(
                new Vector3f(bonePos.x + boneDir.x, 5, bonePos.z + boneDir.z),
                Rgba.OPAQUE_WHITE,
                10,
                PI / 12,
                0.01f
                );
            light.direction = new Vector3f(0, -1, 0);
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
