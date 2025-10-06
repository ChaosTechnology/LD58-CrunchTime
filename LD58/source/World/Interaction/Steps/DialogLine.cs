using ChaosFramework.Graphics.Text;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Interaction.Steps
{
    using Player;

    public class DialogLine
        : InteractionStep
    {
        const float CHAR_SIZE = 0.08f;

        public readonly string text;
        public readonly LayoutInfo layout;

        TextBox textBox;
        bool done;

        public override bool interactionDone => done;

        public DialogLine(Interactor interactor, string text, LayoutInfo layout = null)
            : base(interactor)
        {
            this.text = text;
            this.layout = layout;
        }

        public override void Activate()
        {
            base.Activate();
            textBox = interactor.AddComponent<TextBox>();
            UpdateText(text, layout);
        }

        protected void UpdateText(string text, LayoutInfo layout = null)
        {
            float screenToBoxDistance = 0.1f;
            textBox.Update(text, layout ?? LayoutInfo.TOP_LEFT, new Vector2f(0, -1 + screenToBoxDistance), new Vector2f(0, -1), CHAR_SIZE);
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            interactor.parent.scene.game.input.AddHandler<InputPushEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Interaction, KeyDown);
        }

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

        protected override void DoDispose()
        {
            base.DoDispose();
            textBox?.Dispose();
        }
    }
}
