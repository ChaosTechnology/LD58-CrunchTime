using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;
using System.Linq;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Inventory;
    using LD58.World.Constants;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class PrepareBreakfast
        : Objective
    {
        const int CONSUMED_FOOR_REQUIREMENT = 2;
        const int CONSUMED_BEVERAGE_REQUIREMENT = 1;

        DoorFrame kitchenDoor;

        protected override string GetText()
            => "Grab some grub.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            Table table = interactible as Table;
            if (table != null && table.GetName() == "Kitchen Table")
            {
                interactor.AddInteraction(
                    new ChooseItemsDialog(
                        interactor,
                        interactor.parent.inventory.CopyBag().Filter(Traits.Food | Traits.Dish | Traits.Beverage),
                        "Let's prepare breakfast.",
                        "Yummy!",
                        CheckComplete,
                        CommonSense.FOOD_NEEDS_DISH
                        )
                    );

                return true;
            }

            DoorFrame kitchenDoor = interactible as DoorFrame;
            if (kitchenDoor != null)
            {
                string unmentRequirement = GetUnmentRequirement(interactor);
                if (unmentRequirement != null)
                {
                    interactor.AddInteraction(new DialogLine(interactor, unmentRequirement));
                    return true;
                }
            }

            return base.Interact(interactor, interactible, interactAt);
        }

        public override void Step(Interactor interactor, WorldObject steppedOn)
        {
            DoorFrame door = steppedOn as DoorFrame;
            if (door?.GetName() == "Dining Room" && door.OnDoorMat(interactor.parent.position))
                (kitchenDoor = door).Lock();
            base.Step(interactor, steppedOn);
        }

        string GetUnmentRequirement(Interactor interactor)
        {
            if (interactor.parent.inventory.Where(item => item.item.traits.HasFlag(Traits.Food | Traits.Consumed)).Sum(item => item.count) < CONSUMED_FOOR_REQUIREMENT)
                return "I'm still hungry.";

            if (interactor.parent.inventory.Where(item => item.item.traits.HasFlag(Traits.Beverage | Traits.Consumed)).Sum(item => item.count) < CONSUMED_BEVERAGE_REQUIREMENT)
                return "My throat is dry.";

            return null;
        }

        void CheckComplete(Interactor interactor, ItemBag selectedItems)
        {
            foreach (ItemBag.ItemCount item in selectedItems)
                for (int i = 0; i < item.count; ++i)
                {
                    interactor.parent.inventory.Remove(item.item);
                    interactor.parent.inventory.AddItem(new Item(
                        item.item.displayName,
                        item.item.traits | Traits.Consumed | Traits.Invisible
                        ));
                }

            if (GetUnmentRequirement(interactor) == null)
            {
                kitchenDoor?.Unlock();
                scene.SetObjective<GetToWork>();
            }
        }
    }
}
