using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using System;
using System.Text;

namespace LD58.World.Interaction.Steps
{
    using Player;
    using Option = Tuple<string, InteractionStep[]>;

    public class Choice
        : DialogLine
    {
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
                bldr.Append(option.Item1);
            }
            return bldr.ToString();
        }

        Option[] options;
        bool chosen;
        string baseText;

        int selection = 0;

        public Choice(Interactor interactor, string text, params Option[] options)
            : base(interactor, CreateText(text, options, 0))
        {
            this.baseText = text;
            this.options = options;
        }

        public override bool interactionDone => chosen;

        public override void SetUpdateCalls()
            => interactor.parent.scene.game.input.AddHandler<InputPushEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Interaction, KeyDown);

        bool KeyDown(InputPushEvent<Keyboard.Key> e)
        {
            switch (e.axis.key)
            {
                case Keyboard.Keys.Space:
                    interactor.AddInteraction(options[selection].Item2);
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
            text.UpdateText(
                interactor.parent.scene.game.textFont,
                CreateText(baseText, options, selection),
                text.geometry.args.layout
                );
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            foreach (Option option in options)
                if (option?.Item2 != null)
                    foreach (InteractionStep step in option.Item2)
                        step?.Dispose();
        }
    }
}
