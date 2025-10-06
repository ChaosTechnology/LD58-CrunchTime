using SysCol = System.Collections.Generic;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;
    using Inventory;

    class GettingDressed
        : Objective
    {
        static readonly RequiredItemsSelection.Requirement[] requirements
            = new RequiredItemsSelection.Requirement[]
        {
            new RequiredItemsSelection.Requirement(Traits.ClothingBottom, 1, "Need some bottom clothing piece."),
            new RequiredItemsSelection.Requirement(Traits.ClothingTop, 1, "Can't go topless"),
            new RequiredItemsSelection.Requirement(Traits.ClothingFeet, 1, "I still need something for the feet."),
        };

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            Wardrobe wardrobe = interactible as Wardrobe;
            if (wardrobe != null)
            {
                interactor.AddInteraction(new RequiredItemsSelection(interactor, "Choose clothes to wear:", "Put on clothes", requirements));
                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        protected override string GetText()
            => "Get dressed";
    }
}
