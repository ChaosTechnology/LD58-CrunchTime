using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Graphics.OpenGl.Lights;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using static ChaosFramework.Math.Constants;
using static ChaosFramework.Math.Exponentials;

namespace LD58.World.Objects.WorldObjects
{
    using TimeIndicatorColor = System.Tuple<float, Rgba>;

    class WorkItem
        : DecoObject
    {
        static readonly TimeIndicatorColor[] LIGHT_COLORS = new[] {
            new TimeIndicatorColor(5, new Rgba(1, 1, 0, 1)),
            new TimeIndicatorColor(3, new Rgba(2, 1, 0, 1)),
            // blinking
            new TimeIndicatorColor(2.5f, new Rgba(0, 0, 0, 1)),
            new TimeIndicatorColor(2.0f, new Rgba(2, 0, 0, 1)),
            new TimeIndicatorColor(1.5f, new Rgba(0, 0, 0, 1)),
            new TimeIndicatorColor(1.0f, new Rgba(2, 0, 0, 1)),
            new TimeIndicatorColor(0.5f, new Rgba(0, 0, 0, 1)),
            new TimeIndicatorColor(0.0f, new Rgba(2, 0, 0, 1)),
        };

        SpotLight light;
        public bool isRightSide { get; private set; }

        float timeRemaining = 15;
        int indicatorIndex = 0;

        float completionFadeout = float.NaN;

        Rgba currentColor;

        public bool Urgent() => timeRemaining < 2;
        public bool Done() => !float.IsNaN(completionFadeout);

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            isRightSide = CreateParameters.RequireAs<Rig.Bone, bool>(args).v2;
            Vector3f bonePos = bone.GetPosition();
            Vector3f boneDir = bone.GetDirection().x0z;
            Vector3f boneSide = new Vector3f(-boneDir.z, 0, boneDir.x);
            light = new SpotLight(
                new Vector3f(bonePos.x, 5, bonePos.z) + boneDir * 1.5f - boneSide * (isRightSide ? 2 : 0),
                Rgba.OPAQUE_BLACK,
                10,
                PI / 12,
                0.1f
                );
            light.direction = new Vector3f(0, -1, 0);
            scene.lights.Add(light);
        }

        public void Complete()
            => completionFadeout = 1;

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        { }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            scene.updateLayers[(int)UpdateLayers.ObjectLogic].Add(Update);
        }

        void Update()
        {
            if (indicatorIndex < LIGHT_COLORS.Length - 1 && timeRemaining < LIGHT_COLORS[indicatorIndex].Item1)
                indicatorIndex++;

            currentColor += (LIGHT_COLORS[indicatorIndex].Item2 - currentColor) * EaseIn(ftime * 15);
            light.color = currentColor * (float.IsNaN(completionFadeout) ? 1 : completionFadeout);

            timeRemaining -= ftime;
            if ((completionFadeout -= ftime * 5) <= 0)
                Dispose();
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            light.Dispose();
        }
    }
}
