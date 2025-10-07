using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Inventory;
    using Objects.WorldObjects;
    using Objects;
    using Player;

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
                    requirements, Complete
                    ));

                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        protected override string GetText()
            => "Get out to work.";

        void Complete(SysCol.Dictionary<Item, int> selectedItems)
        {
            Stage stage = new Stage(scene.game, scene.game.assetSource, "office");
            stage.doUpdate = false;
            stage.doDraw = false;
            stage.SetObjective<FindWorkspace>();


            Player oldPlayer = null;
            foreach (Player s in scene.EnumerateChildren<Player>(false))
                oldPlayer = s;

            Player player = null;
            foreach (Player s in stage.EnumerateChildren<Player>(false))
                player = s;

            player.inventory.CarryOver(oldPlayer.inventory);

            scene.game.scenes.Add(stage);
        }
    }
}
