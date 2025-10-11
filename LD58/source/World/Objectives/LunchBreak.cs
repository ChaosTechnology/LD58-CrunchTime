using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Constants;
    using Interaction.Steps;
    using Inventory;
    using Objects;
    using Objects.WorldObjects;
    using Player;

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
                foreach (ItemBag.Entry i in p.inventory)
                    if (i.item.traits.HasFlag(Traits.Consumed))
                        p.inventory.Remove(i.item, true);
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
                else if (!interactor.parent.inventory.Contains(Traits.Hydrated, 3))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Gotta drink something.")
                        );
                else if (!interactor.parent.inventory.Contains(Traits.WellFed, 6))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "I'm hungry, gotta eat something.")
                        );
                else if (!interactor.parent.inventory.Contains(Traits.Caffeinated, 1))
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
                    new ChooseItemsDialog(
                        interactor,
                        interactor.parent.inventory.CopyBag().Filter(Traits.Food | Traits.Beverage | Traits.Dish | Traits.LiquidContainer),
                        "Consume nourishments?.",
                        "Eat.",
                        Consume,
                        CommonSense.FOOD_NEEDS_DISH,
                        CommonSense.BEVERAGE_NEEDS_CONTAINER
                        )
                    );

                return true;
            }

            return false;
        }

        void Consume(Interactor interactor, ItemBag selectedItems)
        {
            foreach (ItemBag.Entry consumed in selectedItems)
                for (int i = 0; i < consumed.count; ++i)
                {
                    interactor.parent.inventory.Remove(consumed.item);
                    if (!consumed.item.traits.HasFlag(Traits.Dish))
                        interactor.parent.inventory.AddItem(
                            new Item(
                                $"Consumed {consumed.item.displayName}",
                                consumed.item.traits | Traits.Invisible | Traits.Consumed
                                )
                            );
                }
        }

        void Complete(Interactor interactor)
            => scene.SetObjective<ConsiderDoWork2>();
    }
}
