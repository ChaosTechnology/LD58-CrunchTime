using ChaosFramework.Math.Vectors;
using LD58.World.Interaction.Steps;
using LD58.World.Inventory;
using LD58.World.Objects.WorldObjects;
using LD58.World.Objects;
using LD58.World.Player;

namespace LD58.World.Objectives
{
    class GetToWork
        : Objective
    {
        static readonly RequiredItemsSelection.Requirement[] requirements
            = new RequiredItemsSelection.Requirement[]
        {
            new RequiredItemsSelection.Requirement(Traits.OpensApartmentDoor, 1, "The door is still locked."),
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
                    requirements, _ => interactor.parent.scene.SetObjective<PrepareBreakfast>()
                    ));

                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        protected override string GetText()
            => "Get out to work.";
    }
}
