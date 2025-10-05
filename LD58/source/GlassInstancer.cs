using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math;
using ChaosUtil.Platform.Paths;

namespace LD58
{
    class GlassInstancer : InstancingAttribute, Transparent
    {
        protected WorldScene scene { get; private set; }

        public ShaderContainer.Entry shader { get; protected set; }
        public MaterialContainer.Entry material { get; protected set; }
        public MeshContainer.Entry mesh { get; protected set; }

        public GlassInstancer() { }

        public GlassInstancer(
            int maxInstances,
            string meshSource,
            string overrideEffect = null,
            params string[] customRegisters
            ) : base(
                  maxInstances,
                  Normalization.NormalizeRelative(meshSource),
                  overrideEffect == null ? null : Normalization.NormalizeRelative(overrideEffect),
                  customRegisters
                  )
        { }

        protected GlassInstancer(int maxInstances, object[] creationParams)
            : base(maxInstances, creationParams)
        { }

        public override void Initialize(Graphics graphics, int maxInstances, object[] parameters)
        {
            scene = Context<WorldScene>();
            scene.transparencyRenderer.transparents.Add(this);

            if (parameters != null)
            {
                informer = new MatrixInstancer(scene.game.graphics, (string[])parameters[2], maxInstances);
                mesh = scene.game.meshes.Load((string)parameters[0], scene);
                shader = parameters[1] == null
                    ? scene.game.shaders.Load("shaders/glass.fx", scene)
                    : scene.game.shaders.Load((string)parameters[1], scene);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            scene.transparencyRenderer.transparents.Add(this);
        }

        void Transparent.PrepareVertices()
        { }

        void Transparent.DrawMask(TransparencyRenderer renderer)
        {
            scene.view.SetValues(renderer.maskEffect, Matrix.IDENTITY);
            mesh.content.DrawInstanced(renderer.maskEffect, "MeshInstanced", informer);
        }

        void Transparent.DrawTransparent(TransparencyRenderer renderer)
        {
            scene.view.SetValues(shader, Matrix.IDENTITY);
            renderer.SetScreenSamplingValues(shader);
            mesh.content.DrawInstanced(shader, "Glass", informer);
        }

        public override void SetDrawCalls()
        { }
    }
}
