using ChaosFramework.Collections;
using ChaosFramework.Graphics.Colors;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Graphics.Text.Formatting;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using ChaosUtil.Primitives;
using System.Linq;
using System.Text;
using static ChaosFramework.Math.Clamping;
using SysCol = System.Collections.Generic;

namespace LD58.World.Interaction.Steps
{
    using Inventory;
    using Player;

    public class ChooseItemsDialog
        : DialogLine
    {
        // TODO: Add option for OPTIONALLY consuming items
        //       e.g. interact multiple times and check completion elsewhere
        //            Interaction 1: eat egg with plate
        //            Interaction 2: eat bacon (with new plate?) & drink coffee

        // TODO: Add vertical scroll for very full inventories

        public struct Requirement
        {
            public delegate bool IsRequirementFulfilled(Interactor interactor, ItemBag selectedItems);

            public readonly string failureText;
            public readonly IsRequirementFulfilled fulfilled;

            public Requirement(string text, IsRequirementFulfilled fulfilled)
            {
                this.failureText = text;
                this.fulfilled = fulfilled;
            }
        }

        public delegate void SuccessCallback(Interactor interactor, ItemBag selectedItems);

        const string NO_STOCK = "No suitable items collected.";
        const string CANCEL = "Leave";

        static readonly Rgba CHANGE_AMOUNT_DISABLED_COLOR = new Rgba(new Rgb(0.2f), 1);

        static bool IsVisible(ItemBag.ItemCount item)
            => !item.item.traits.HasFlag(Traits.Invisible);

        int cursor = 0;
        bool done = false;

        readonly ItemBag selection;
        readonly SuccessCallback callback;

        readonly ItemBag available;
        readonly Requirement[] requirements;

        readonly string prompt;
        readonly string acceptText;

        public ChooseItemsDialog(
            Interactor interactor,
            ItemBag available,
            string prompt,
            string acceptText,
            SuccessCallback callback,
            params Requirement[] requirements
            )
            : base(interactor, "" /* initialized on activation */)
        {
            this.prompt = prompt;
            this.acceptText = acceptText;
            this.callback = callback;
            this.requirements = requirements ?? Array<Requirement>.empty;
            this.available = available.Filter(IsVisible);
            selection = new ItemBag();
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
                    if (cursor == available.numItemKinds + 1)
                        done = true;

                    if (cursor == available.numItemKinds
                        && selection.NotEmpty()
                        && requirements.All(req => req.fulfilled(interactor, selection))
                        )
                    {
                        done = true;
                        callback(interactor, selection);
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
            cursor = ChaosFramework.Math.Modulus.Mod(cursor + delta, available.numItemKinds + 2);
            EnforceRequirements();
        }

        void DeltaCount(int delta)
        {
            if (cursor >= available.numItemKinds) // cursor is on accept or leave option
                return;

            ItemBag.ItemCount a = available.ElementAt(cursor);
            ItemBag.ItemCount s = selection.FirstOrDefault(x => x.item == a.item);
            int? availableCount = s?.count;
            delta = Clamp(-s?.count ?? 0, a.count, delta);

            if (delta > 0)
                while (--delta >= 0 && a.count > (s?.count ?? 0))
                    selection.Add(a.item);
            else
                while (++delta <= 0 && s?.count > 0)
                    selection.Remove(a.item);

            EnforceRequirements();
        }

        void CalculateLineWidth(string text, ref float minWidth)
        {
            TextGeometryDescription args = new TextGeometryDescription(
                interactor.parent.scene.game.textFont.content,
                text,
                layout
                );
            TextGeometry geo = new TextGeometry(args);
            minWidth = Max(minWidth, geo.textBounds.width);
        }

        void WriteChoiceLine(StringBuilder bldr, ItemBag.ItemCount available, int selectedCount)
        {
            if (selectedCount <= 0) bldr.Append(ColoredTextScope.GetColorCode(CHANGE_AMOUNT_DISABLED_COLOR));
            bldr.Append("< ");
            if (selectedCount <= 0) bldr.Append(ColoredTextScope.RESET_COLOR_CODE);

            bldr.Append(selectedCount);

            if (selectedCount >= available.count) bldr.Append(ColoredTextScope.GetColorCode(CHANGE_AMOUNT_DISABLED_COLOR));
            bldr.Append(" >");
            if (selectedCount >= available.count) bldr.Append(ColoredTextScope.RESET_COLOR_CODE);

            bldr.Append('\t');
            bldr.Append(available.item.displayName);
        }

        void EnforceRequirements()
        {
            float minWidth = 0;
            CalculateLineWidth(prompt, ref minWidth);
            CalculateLineWidth(NO_STOCK, ref minWidth);
            CalculateLineWidth($">{CANCEL}", ref minWidth);

            string failedRequirement = null;
            foreach (Requirement req in requirements)
            {
                if (failedRequirement == null && !req.fulfilled(interactor, selection))
                    failedRequirement = req.failureText;

                CalculateLineWidth($">{req.failureText}", ref minWidth);
            }

            StringBuilder bldr = new StringBuilder();
            foreach (ItemBag.ItemCount choice in available)
                for (int count = 0; count < choice.count; ++count)
                {
                    WriteChoiceLine(bldr, choice, count);
                    CalculateLineWidth(bldr.ToString(), ref minWidth);
                    bldr.Clear();
                }

            bldr.AppendLine(prompt);

            SysCol.Dictionary<Item, int> itemCounts = selection.ToDictionary(x => x.item, x => x.count);
            int i = 0;
            foreach (ItemBag.ItemCount available in available)
            {
                bldr.AppendLine();
                int selectedCount;
                itemCounts.TryGetValue(available.item, out selectedCount);
                using (new ColoredTextScope(bldr, i++ == cursor ? new Rgba(1, 1, 0, 1) : Rgba.OPAQUE_WHITE))
                    WriteChoiceLine(bldr, available, selectedCount);
            }

            if (i == 0)
            {
                bldr.AppendLine();
                using (new ColoredTextScope(bldr, new Rgba(0.5f, 0.5f, 0.5f, 1)))
                    bldr.Append(NO_STOCK);
            }

            bldr.AppendLine();
            bldr.AppendLine();

            if (cursor == i)
                bldr.Append('>');

            bool canAccept = failedRequirement == null && selection.Any();
            using (new ColoredTextScope(bldr, canAccept ? Rgba.OPAQUE_WHITE : new Rgba(0.5f, 0.5f, 0.5f)))
                bldr.AppendLine(failedRequirement ?? acceptText);

            if (cursor == i + 1)
                bldr.Append('>');
            bldr.AppendLine(CANCEL);

            UpdateText(bldr.ToString(), minWidth);
        }
    }
}
