using ChaosFramework.Math.Vectors;
using System.Linq;
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
        static readonly RequiredItemsSelection.Requirement[] requirements
            = new RequiredItemsSelection.Requirement[]
        {
            new RequiredItemsSelection.Requirement(Traits.OpensDoor, 1, "The door is still locked."),
            new RequiredItemsSelection.Requirement(Traits.StartsCar, 1, "Yeah, but how do I start my car?"),
        };

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            DoorFrame wardrobe = interactible as DoorFrame;
            if (wardrobe != null)
            {
                interactor.AddInteraction(new RequiredItemsSelection(
                    interactor,
                    "I guess I'll leave for work now... let's check my inventory:",
                    "Get going already!",
                    requirements,
                    Complete
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
