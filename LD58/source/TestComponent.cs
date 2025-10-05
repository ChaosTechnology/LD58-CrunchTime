using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Math;
using ChaosFramework.Graphics.OpenGl.AssetContainers;

namespace LD58
{
    internal class TestComponent : Component<WorldScene>
    {
        ShaderContainer.Entry shader;
        MeshContainer.Entry mesh;
        MaterialContainer.Entry material;

        protected override void Create(CreateParameters cparams)
        {
            shader = scene.game.graphics.shaders.normalMap;
            mesh = scene.game.meshes.Load("objects/long counter.gmdl", this);
            material = scene.game.materials.Load("objects/kitchen.mat", this);
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            scene.drawLayers[(int)DrawLayers.World].Add(() => Draw("World"));
            scene.drawLayers[(int)DrawLayers.Material].Add(() => Draw("Material"));

        }

        void Draw(string pass)
        {
            scene.view.SetValues(shader, Matrix.RotationY(ftime.totalTime));
            material.content.SetValues(shader);
            mesh.content.Draw(shader, pass);
        }
    }
}
