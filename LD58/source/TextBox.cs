using ChaosFramework.Components;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;

namespace LD58
{
    public class TextBox : Component<WorldScene>
    {
        const float PADDING = 0.75f;

        ShaderContainer.Entry shader;
        Matrix boxTransform, invBoxTransform;
        Bounds2f boxBounds;

        Text text;
        float charSize;

        protected override void Create(CreateParameters cparams)
        {
            shader = scene.game.shaders.Load("shaders/dialog.fx", this);
            text = new Text(scene.game.textRenderer, 4096);
        }

        public void Update(string value, LayoutInfo layout, Vector2f position, Vector2f anchor, float charSize)
        {
            this.charSize = charSize;

            text.UpdateText(scene.game.textFont, value, layout);

            boxBounds = new Bounds2f(text.geometry.textBounds.low, text.geometry.textBounds.high);
            boxBounds.low -= PADDING;
            boxBounds.high += PADDING;

            Matrix offset = Matrix.Translation(
                position.x - anchor.x * boxBounds.width * charSize * 0.5f,
                position.y - anchor.y * boxBounds.height * charSize * 0.5f,
                0);

            this.text.transform = Matrix.Translation(
                                    -boxBounds.center.x,
                                    -boxBounds.center.y + 0.25f /* arbitrary offset to make it LOOK centered */,
                                    0
                                )
                                * Matrix.Scaling(charSize)
                                * offset
                                ;
            text.color = Rgba.OPAQUE_WHITE;

            boxTransform = Matrix.Scaling(boxBounds.width * charSize * 0.5f, boxBounds.height * charSize * 0.5f, 1)
                         * offset
                         ;
            invBoxTransform = Matrix.Invert(boxTransform);
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            scene.drawLayers[(int)DrawLayers.FillInstancers].Add(AddTexts);
            scene.drawLayers[(int)DrawLayers.HUD].Add(DrawBox);
        }

        void AddTexts()
            => scene.game.textBuffer.Add(text);

        void DrawBox()
        {
            scene.fullScreenView.SetValues(shader, boxTransform, invBoxTransform);
            shader.SetValue("dimensions", boxBounds.size * charSize * 18);
            shader.BeginPass("Dialog");
            Sprite.DrawPositionOnly(shader.content.graphics);
            shader.EndPass();
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            text.Dispose();
        }
    }
}
