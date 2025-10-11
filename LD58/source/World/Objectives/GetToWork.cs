using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Inventory;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class GetToWork
        : Objective
    {
        static bool CanOpenDoor(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelection)
            => traitsFromSelection.ContainsKey(Traits.OpensDoor);

        static bool CanDriveCar(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelection)
            => traitsFromSelection.ContainsKey(Traits.StartsCar);

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            DoorFrame wardrobe = interactible as DoorFrame;
            if (wardrobe != null)
            {
                interactor.AddInteraction(new TransformItemsDialog(
                    interactor,
                    "I guess I'll leave for work now... let's check my inventory:",
                    "Get going already!",
                    Traits.OpensDoor | Traits.StartsCar,
                    Complete,
                    new TransformItemsDialog.Requirement("The door is still locked.", CanOpenDoor),
                    new TransformItemsDialog.Requirement("Yeah, but how do I start my car?", CanDriveCar)
                    ));

                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        protected override string GetText()
            => "Get out to work.";

        void Complete(Interactor interactor, SysCol.Dictionary<Item, int> selectedItems)
            => scene.game.SwitchScene<FindWorkspace>(interactor, "office");
    }
}
