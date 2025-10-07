using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Inventory;
    using Objects.WorldObjects;
    using Player;
    using Constants;

    class LunchBreak
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            myDesk = scene.Find<OfficeTable>("MyDesk");

            foreach (Player p in scene.EnumerateChildren<Player>(false))
            {
                p.inventory.AddItem(KnownItems.HELD_IN_POOP);
                foreach (System.Tuple<Item, int> i in p.inventory)
                    if (i.Item1.traits.HasFlag(Traits.Consumed))
                        p.inventory.Remove(i.Item1, true);
            }
        }

        protected override string GetText()
            => "Take a break.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
            => InteractWithDesk(interactor, interactible as OfficeTable)
            || InteractWithTable(interactor, interactible as CheapTable)
            || interactible.Interact(interactor, interactAt);

        bool InteractWithDesk(Interactor interactor, OfficeTable desk)
        {
            if (desk == myDesk && myDesk.FacingScreenZero(interactor.parent.direction))
            {
                if (interactor.parent.inventory.Contains(KnownItems.HELD_IN_POOP))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "I really need to hit the bathroom.")
                        );
                else if (!interactor.parent.inventory.Contains(Traits.Hydrated))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Gotta drink something.")
                        );
                else if (!interactor.parent.inventory.Contains(Traits.WellFed))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Gotta eat something.")
                        );
                else if (!interactor.parent.inventory.Contains(Traits.Caffeinated))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Man, I really need some caffeine.")
                        );
                else
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Well, enough slacking off..."),
                        new CustomAction(interactor, Complete)
                        );

                return true;
            }

            return false;

        }

        bool InteractWithTable(Interactor interactor, CheapTable table)
        {
            if (table != null && table.GetName() == "Break Room")
            {
                interactor.AddInteraction(
                    new RequiredItemsSelection(
                        interactor,
                        "Consume nourishments?.",
                        "Eat.",
                        new[] {
                            new RequiredItemsSelection.Requirement(Traits.Food, 6, "I'm hungry, need more food."),
                            new RequiredItemsSelection.Requirement(Traits.Dish, 1, "I still need a plate or something."),
                            new RequiredItemsSelection.Requirement(Traits.Beverage, 3, "My mouth is so dry, need more drinks."),
                            new RequiredItemsSelection.Requirement(Traits.LiquidContainer, 1, "Can't just pour my drink on the table."),
                            },
                        Consume
                        )
                    );

                return true;
            }

            return false;
        }

        void Consume(Interactor interactor, SysCol.Dictionary<Item, int> selectedItems)
        {
            foreach (SysCol.KeyValuePair<Item, int> consumed in selectedItems)
                for (int i = 0; i < consumed.Value; ++i)
                {
                    interactor.parent.inventory.Remove(consumed.Key);
                    if (!consumed.Key.traits.HasFlag(Traits.Dish))
                        interactor.parent.inventory.AddItem(
                            new Item(
                                $"Consumed {consumed.Key.displayName}",
                                consumed.Key.traits | Traits.Invisible | Traits.Consumed
                                )
                            );
                }
        }

        void Complete(Interactor interactor)
            => scene.SetObjective<DoWork2>();
    }
}
