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
            new RequiredItemsSelection.Requirement(Traits.CoversBottom, 1, "Need some bottom clothing piece."),
            new RequiredItemsSelection.Requirement(Traits.CoversTop, 1, "Can't go topless"),
            new RequiredItemsSelection.Requirement(Traits.CoversFeet, 1, "I still need something for the feet."),
        };

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            Wardrobe wardrobe = interactible as Wardrobe;
            if (wardrobe != null)
            {
                interactor.AddInteraction(new Choice(interactor, "I can get dressed here...",
                    new Choice.Option("Choose clothes...", new CustomAction(interactor, () =>
                        interactor.AddInteraction(new RequiredItemsSelection(
                            interactor,
                            "Choose clothes to wear:",
                            "Wear this!",
                            requirements, _ => interactor.parent.scene.SetObjective<PrepareBreakfast>()
                            ))
                        )),
                    new Choice.Option("Rummage through this wardrobe...",
                        new CustomAction(interactor, () => interactible.Interact(interactor, interactAt))
                        ),
                    new Choice.Option("Leave")
                    ));
                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        protected override string GetText()
            => "Get dressed";
    }
}
