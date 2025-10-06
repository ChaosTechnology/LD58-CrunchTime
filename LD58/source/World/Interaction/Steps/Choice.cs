using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using System.Text;

namespace LD58.World.Interaction.Steps
{
    using Player;

    public class Choice
        : DialogLine
    {
        public class Option
        {
            public readonly string text;
            public readonly InteractionStep[] steps;

            public Option(string text, params InteractionStep[] steps)
            {
                this.text = text;
                this.steps = steps;
            }
        }

        const char CURSOR_SELECT = '>';
        const char CURSOR_BLANK = ' ';

        static string CreateText(string text, Option[] options, int selection)
        {
            StringBuilder bldr = new StringBuilder(text);
            int i = 0;
            foreach (Option option in options)
            {
                bldr.AppendLine();
                bldr.Append("  ");
                bldr.Append(i++ == selection ? CURSOR_SELECT : CURSOR_BLANK);
                bldr.Append(option.text);
            }
            return bldr.ToString();
        }

        Option[] options;
        bool chosen;
        string prompt;

        int selection = 0;

        public Choice(Interactor interactor, string prompt, params Option[] options)
            : base(interactor, CreateText(prompt, options, 0))
        {
            this.prompt = prompt;
            this.options = options;
        }

        public override bool interactionDone => chosen;

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            interactor.parent.scene.game.input.AddHandler<InputPushEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Interaction, KeyDown);
        }

        bool KeyDown(InputPushEvent<Keyboard.Key> e)
        {
            switch (e.axis.key)
            {
                case Keyboard.Keys.Space:
                    interactor.AddInteraction(options[selection].steps);
                    options[selection] = null; // it is now the interactor's responsibility to discard these
                    chosen = true;
                    return true;

                case Keyboard.Keys.W:
                    DeltaChoice(-1);
                    return true;

                case Keyboard.Keys.S:
                    DeltaChoice(1);
                    return true;

                default:
                    return false;

            }
        }

        void DeltaChoice(int delta)
        {
            selection = ChaosFramework.Math.Modulus.Mod(selection + delta, options.Length);
            UpdateText(CreateText(prompt, options, selection));
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            foreach (Option option in options)
                if (option?.steps != null)
                    foreach (InteractionStep step in option.steps)
                        step?.Dispose();
        }
    }
}
