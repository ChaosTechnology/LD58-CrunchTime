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
        readonly ShaderContainer.Entry shader;
        readonly Matrix boxTransform, invBoxTransform;

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

            Bounds2f bounds = this.text.geometry.textBounds;
            this.text.transform = Matrix.Translation((bounds.topLeft - bounds.bottomRight) / 2, 0)
                                * Matrix.Scaling(0.05f, 0.05f, 1)
                                ;
            this.text.color = Rgba.OPAQUE_WHITE;

            boxTransform = Matrix.Translation(1, -1, 0)
                         * Matrix.Scaling(0.5f * bounds.size, 1)
                         * this.text.transform;
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
