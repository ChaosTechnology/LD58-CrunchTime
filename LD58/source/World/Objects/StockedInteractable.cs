using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using System.Linq;
using SysCol = System.Collections.Generic;
using ChaosUtil.Primitives;

namespace LD58.World.Objects
{
    using Interaction.Steps;
    using Inventory;
    using Interaction;
    using Player;

    public abstract class StockedInteractible
        : Interactible
    {
        protected readonly ItemBag stock = new ItemBag();

        protected virtual string displayName => name;

        protected virtual string prompt => "Take something?";
        protected virtual string promptEmpty => "There nothing in here.";
        protected virtual string takeOption => "Take";

        public StockedInteractible(uint width = 1, uint height = 1)
            : base(width, height)
        { }

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            foreach (Item item in GetInitialStock())
                stock.Add(item);
        }

        protected abstract SysCol.IEnumerable<Item> GetInitialStock();

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            interactor.AddInteraction(
                stock.Any()
                ? new ChooseItemsDialog(
                    interactor,
                    stock,
                    prompt,
                    takeOption,
                    SuccessCallback,
                    null
                    )
                : new DialogLine(interactor, promptEmpty)
                );

            return true;
        }

        protected virtual SysCol.IEnumerable<InteractionStep> PrependSteps(Interactor interactor)
            => Array<InteractionStep>.empty;

        protected virtual void SuccessCallback(Interactor interactor, ItemBag selectedItems)
        {
            foreach (ItemBag.Entry i in selectedItems)
                for (int x = 0; x < i.count; ++x)
                {
                    stock.Remove(i.item);
                    interactor.parent.inventory.AddItem(i.item);
                }
        }
    }
}
