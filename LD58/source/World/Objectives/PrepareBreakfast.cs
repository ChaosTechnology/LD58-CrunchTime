using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Inventory;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class PrepareBreakfast
        : Objective
    {
        static readonly RequiredItemsSelection.Requirement[] req = new RequiredItemsSelection.Requirement[]
        {
            new RequiredItemsSelection.Requirement(Inventory.Traits.Dish, 1, "Can't prepare it in the toilet bowl..."),
            new RequiredItemsSelection.Requirement(Inventory.Traits.Food, 2, "An empty plate won't cut it."),
            new RequiredItemsSelection.Requirement(Inventory.Traits.Beverage, 1, "Maybe add some liquids.")
        };

        protected override string GetText()
            => "Grab some grub.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            Table table = interactible as Table;
            if (table != null && table.GetName() == "Kitchen Table")
            {
                interactor.AddInteraction(
                    new RequiredItemsSelection(interactor, "Let's prepare breakfast.", "Yummy!", req, Complete)
                    );

                return true;
            }

            return base.Interact(interactor, interactible, interactAt);
        }

        void Complete(SysCol.Dictionary<Item, int> selectedItems)
            => scene.SetObjective<GetToWork>();
    }
}
