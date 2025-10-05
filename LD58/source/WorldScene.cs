using ChaosFramework.Components;
using ChaosFramework.Graphics;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Graphics.OpenGl.Lights;
using ChaosFramework.Graphics.OpenGl.Lights.Intrinsic;
using ChaosFramework.Graphics.OpenGl.PostProcessors;
using ChaosFramework.Math.Vectors;
using LD58.source;
using OpenTK.Graphics.OpenGL;
using static ChaosFramework.Math.Constants;

namespace LD58
{
    public class WorldScene : Scene<Game>
    {
        protected AntiAliasing antiEdger = new AntiAliasing();
        public TransparencyRenderer transparencyRenderer;

        public readonly Camera view;
        public readonly LightSet lights;
        public readonly DeferredShader shader;

        Light theFamousInsideSun;
        public readonly InstancingManagerContainer<WorldScene> instancers;

        public WorldScene(Game game)
            : base(game, typeof(UpdateLayers), typeof(DrawLayers))
        {
            view = new Camera();

            view.Update(
                new Vector3f(0, 0, -3),
                new Vector3f(0, 0, 1),
                new Vector3f(0, 1, 0),
                float.NaN,
                float.NaN,
                PI_QUART / 2,
                screenRatio: game.graphics.ratio
                );

            lights = new LightSet();
            shader = new DeferredShader(
                base.game.graphics,
                view,
                new Vector2i(
                    (game.settings.deferredShaderResolution.x <= 0) ? game.panel.Width : game.settings.deferredShaderResolution.x,
                    (game.settings.deferredShaderResolution.y <= 0) ? game.panel.Height : game.settings.deferredShaderResolution.y
                    ),
                lights,
                new DeferredShaderIntrinsicLights[] { new DirectionalLightIntrinsics(1) },
                new LightInstancerBase[] { new PointLightInstancer(game.graphics, 128) }
                );

            theFamousInsideSun = new DirectionalLight(
                new Vector3f(1, -5, 1),
                new Rgba(1, 1, 1, 1)
                );

            transparencyRenderer = new TransparencyRenderer(
                base.game.graphics,
                (int)(shader.width * game.settings.transparencyScale),
                (int)(shader.height * game.settings.transparencyScale),
                shader.depthBuffer,
                game.settings.transparencyLayers
                );

            instancers = new InstancingManagerContainer<WorldScene>(this, game.graphics);
            instancers.CreateInstancers(_ => true);

            antiEdger = new AntiAliasing();

            lights.Add(theFamousInsideSun);
            AddComponent<Background>();
        }

        void UpdateView() => view.Update(view.Position, view.Direction, view.Up, float.NaN, float.NaN, float.NaN, game.graphics.ratio);

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            drawLayers[(int)DrawLayers.UpdateView].Add(UpdateView);
            instancers.SetDrawCalls(drawLayers[(int)DrawLayers.ResetInstancers], drawLayers[(int)DrawLayers.FillInstancers]);
            drawLayers[(int)DrawLayers.BeginWorld].Add(shader.BeginWorld);
            drawLayers[(int)DrawLayers.BeginMaterial].Add(shader.BeginMaterial);
            drawLayers[(int)DrawLayers.RenderDeferredShader].Add(shader.Render);
            drawLayers[(int)DrawLayers.PrepareBackground].Add(PrepareBackground);
            drawLayers[(int)DrawLayers.Transparents].Add(RenderTransparents);
            drawLayers[(int)DrawLayers.Postprocessing].Add(AntiEdgy);
        }

        void RenderTransparents()
            => transparencyRenderer.Render(shader);

        void PrepareBackground()
        {
            GL.Viewport(
                0,
                game.panel.Height - game.panel.ClientSize.Height,
                game.panel.ClientSize.Width,
                game.panel.ClientSize.Height
                );
            Graphics.ThrowErrors();
            game.graphics.stateTracker.BindFramebuffer(FramebufferTarget.Framebuffer, null);
        }

        void AntiEdgy()
        {
            GL.Viewport(0, game.panel.Height - game.panel.ClientSize.Height, game.panel.ClientSize.Width, game.panel.ClientSize.Height);
            Graphics.ThrowErrors();
            game.graphics.stateTracker.BindFramebuffer(FramebufferTarget.Framebuffer, null);
            antiEdger.normalFactor = 6f;
            antiEdger.positionFactor = .6f;
            antiEdger.Apply(shader);
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            shader?.Dispose();
            theFamousInsideSun.Dispose();
            transparencyRenderer?.Dispose();
        }
    }
}
