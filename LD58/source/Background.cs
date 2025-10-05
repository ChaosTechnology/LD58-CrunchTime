using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.Math.Vectors;
using static ChaosFramework.Math.Trigonometry;

namespace LD58.source
{
    internal class Background : Component<WorldScene>
    {
        ShaderContainer.Entry shader;

        protected override void Create(CreateParameters cparams)
        {
            shader = scene.game.shaders.Load("shaders/background.fx", this);
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            scene.drawLayers[(int)DrawLayers.Background].Add(Draw);
        }

        void Draw()
        {
            shader.SetValue("time_sanity", new Vector2f(ftime.totalTime, Sin(ftime.totalTime) * 0.5f + 0.5f));
            shader.BeginPass("Screen");
            Sprite.DrawFullscreen(scene.game.graphics);
            shader.EndPass();
        }
    }
}
