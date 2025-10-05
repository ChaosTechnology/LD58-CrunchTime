using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.Graphics.OpenGl.Text;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using ChaosFramework.Math;

namespace LD58.World.Interaction.Steps
{
    using Player;

    public class DialogLine
        : InteractionStep
    {
        const float CHAR_SIZE = 0.08f;
        const float PADDING = 0.75f;

        readonly ShaderContainer.Entry shader;
        readonly Matrix boxTransform, invBoxTransform;
        readonly Bounds2f boxBounds;

        protected readonly Text text;

        bool done;

        Graphics graphics => interactor.parent.scene.game.graphics;

        public override bool interactionDone => done;

        public DialogLine(Interactor interactor, string text, LayoutInfo layout = null)
            : base(interactor)
        {
            this.shader = interactor.parent.scene.game.shaders.Load("shaders/dialog.fx", this);

            this.text = new Text(interactor.parent.scene.game.textRenderer, text.Length);
            this.text.UpdateText(interactor.parent.scene.game.textFont, text, layout ?? LayoutInfo.TOP_LEFT);

            boxBounds = this.text.geometry.textBounds;

            boxBounds.low -= PADDING;
            boxBounds.high += PADDING;

            float screenToBoxDistance = 0.1f;
            Matrix toBottom = Matrix.Translation(0, -1.0f + boxBounds.height * CHAR_SIZE * 0.5f + screenToBoxDistance, 0);

            this.text.transform = Matrix.Translation(
                                    -boxBounds.center.x,
                                    -boxBounds.center.y + 0.25f /* arbitrary offset to make it LOOK centered */,
                                    0
                                )
                                * Matrix.Scaling(CHAR_SIZE, CHAR_SIZE, 1)
                                * toBottom
                                ;
            this.text.color = Rgba.OPAQUE_WHITE;
            boxTransform = Matrix.Scaling(boxBounds.width * CHAR_SIZE * 0.5f, boxBounds.height * CHAR_SIZE * 0.5f, 1)
                         * toBottom
                         ;
            invBoxTransform = Matrix.Invert(boxTransform);
        }

        public override void SetUpdateCalls()
            => interactor.parent.scene.game.input.AddHandler<InputPushEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Interaction, KeyDown);

        bool KeyDown(InputPushEvent<Keyboard.Key> key)
        {
            switch (key.axis.key)
            {
                case Keyboard.Keys.Space:
                    done = true;
                    return true;

                default:
                    return false;
            }
        }

        public override void SetDrawCalls()
        {
            interactor.parent.scene.drawLayers[(int)DrawLayers.FillInstancers].Add(AddTexts);
            interactor.parent.scene.drawLayers[(int)DrawLayers.HUD].Add(DrawBox);
        }

        void AddTexts()
            => interactor.parent.scene.game.textBuffer.Add(text);

        void DrawBox()
        {
            interactor.parent.scene.fullScreenView.SetValues(shader, boxTransform, invBoxTransform);
            shader.SetValue("dimensions", boxBounds.size * CHAR_SIZE * 18);
            shader.BeginPass("Dialog");
            Sprite.DrawPositionOnly(graphics);
            shader.EndPass();
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            text.Dispose();
        }
    }
}
