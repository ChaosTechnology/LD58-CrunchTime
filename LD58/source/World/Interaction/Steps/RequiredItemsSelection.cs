using System.Linq;
using System.Text;
using ChaosFramework.Collections;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.Text.Formatting;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using SysCol = System.Collections.Generic;
using static ChaosFramework.Math.Clamping;

namespace LD58.World.Interaction.Steps
{
    using Inventory;
    using Player;

    public class RequiredItemsSelection
        : DialogLine
    {
        // TODO: Add option for OPTIONALLY consuming items
        //       e.g. interact multiple times and check completion elsewhere
        //            Interaction 1: eat egg with plate
        //            Interaction 2: eat bacon (with new plate?) & drink coffee

        public struct Requirement
        {
            public Traits trait;
            public readonly int count;
            public string failureText;
            public Requirement(Traits traits, int count, string failureText)
            {
                this.trait = traits;
                this.count = count;
                this.failureText = failureText;
            }
        }

        public delegate void SuccessCallback(Interactor interactor, SysCol.Dictionary<Item, int> selectedItems);

        int cursor = 0;
        bool done = false;

        readonly LinkedList<System.Tuple<Item, int>> available;
        readonly Requirement[] requirements;
        readonly ItemBag selection;
        readonly SuccessCallback callback;

        readonly string prompt;
        readonly string acceptText;

        public RequiredItemsSelection(
            Interactor interactor,
            string prompt,
            string acceptText,
            Requirement[] requirements,
            SuccessCallback callback
            )
            : base(interactor, "" /* initialized on activation */)
        {
            this.prompt = prompt;
            this.acceptText = acceptText;
            this.requirements = requirements;
            this.callback = callback;

            selection = new ItemBag();

            Traits selectionFilter = Traits.None;
            foreach (Requirement requirement in requirements)
                selectionFilter |= requirement.trait;

            available = new LinkedList<System.Tuple<Item, int>>(
                interactor.parent.inventory
                    .Where(x => (x.Item1.traits & selectionFilter) != Traits.None)
                    .Where(x => !x.Item1.traits.HasFlag(Traits.Invisible))
                );
        }

        public override void Activate()
        {
            base.Activate();
            EnforceRequirements();
        }

        public override bool interactionDone => done;

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
                    if (cursor == available.length + 1)
                        done = true;

                    if (cursor == available.length && GetFailedRequirement() == null)
                    {
                        done = true;
                        callback(interactor, selection.ToDictionary(x => x.Item1, x => x.Item2));
                    }

                    return true;

                case Keyboard.Keys.W:
                    DeltaChoice(-1);
                    return true;

                case Keyboard.Keys.S:
                    DeltaChoice(1);
                    return true;


                case Keyboard.Keys.A:
                    DeltaCount(-1);
                    return true;


                case Keyboard.Keys.D:
                    DeltaCount(1);
                    return true;

                default:
                    return false;

            }
        }

        void DeltaChoice(int delta)
        {
            cursor = ChaosFramework.Math.Modulus.Mod(cursor + delta, available.length + 2);
            EnforceRequirements();
        }

        void DeltaCount(int delta)
        {
            if (cursor >= available.length) // cursor is on accept or leave option
                return;

            System.Tuple<Item, int> a = available[cursor];
            System.Tuple<Item, int> s = selection.FirstOrDefault(x => x.Item1 == a.Item1);
            int? availableCount = s?.Item2;
            delta = Clamp(-s?.Item2 ?? 0, a.Item2, delta);

            if (delta > 0)
                while (--delta >= 0 && a.Item2 > (s?.Item2 ?? 0))
                    selection.Add(a.Item1);
            else
                while (++delta <= 0 && s?.Item2 > 0)
                    selection.Remove(a.Item1);

            EnforceRequirements();
        }

        void EnforceRequirements()
        {
            StringBuilder bldr = new StringBuilder(prompt);
            bldr.AppendLine();

            SysCol.Dictionary<Item, int> itemCounts = selection.ToDictionary(x => x.Item1, x => x.Item2);
            int i = 0;
            foreach (System.Tuple<Item, int> available in available)
            {
                bldr.AppendLine();
                int selectedCount;
                itemCounts.TryGetValue(available.Item1, out selectedCount);
                using (new ColoredTextScope(bldr, i++ == cursor ? new Rgba(1, 1, 0, 1) : Rgba.OPAQUE_WHITE))
                {
                    if (selectedCount > 0)
                    {
                        bldr.Append(selectedCount);
                        bldr.Append("x");
                    }
                    bldr.Append('\t');
                    bldr.Append(available.Item1.displayName);
                }
            }

            if (i == 0)
            {
                bldr.AppendLine();
                using (new ColoredTextScope(bldr, new Rgba(0.5f, 0.5f, 0.5f, 1)))
                    bldr.Append("No suitable items collected.");
            }

            bldr.AppendLine();
            bldr.AppendLine();
            if (cursor == i)
                bldr.Append('>');

            string failedRequirement = GetFailedRequirement();
            if (failedRequirement != null)
                using (new ColoredTextScope(bldr, new Rgba(0.5f, 0.5f, 0.5f)))
                    bldr.AppendLine(failedRequirement);
            else
                bldr.AppendLine(acceptText);

            if (cursor == i + 1)
                bldr.Append('>');
            bldr.AppendLine("Leave");

            UpdateText(bldr.ToString());
        }

        string GetFailedRequirement()
        {
            SysCol.Dictionary<Traits, int> traitCounts = selection.CountTraits().ToDictionary(x => x.Item1, x => x.Item2);
            foreach (Requirement req in requirements)
            {
                int count;
                if (!traitCounts.TryGetValue(req.trait, out count) || count < req.count)
                    return req.failureText;
            }

            return null;
        }
    }
}
