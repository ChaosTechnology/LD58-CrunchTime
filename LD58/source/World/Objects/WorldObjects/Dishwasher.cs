using ChaosFramework.Math.Vectors;
using System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Dishwasher.gmdl", "objects/Kitchen.mat")]
    class Dishwasher
        : StockedInteractible
    {
        protected override IEnumerable<Item> GetInitialStock()
        {
            yield return KnownItems.CLEAN_PLATE;
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            if (stock.Contains(KnownItems.CLEAN_PLATE))
                interactor.AddInteraction(
                    new Choice(
                        interactor,
                        "Take a plate?",
                        new Choice.Option("Yes.",
                            new DialogLine(interactor, "Took plate."),
                            new AddItem(interactor, KnownItems.CLEAN_PLATE)
                            ),
                        new Choice.Option("No")
                        )
                    );
            else
                interactor.AddInteraction(
                    new DialogLine(interactor, "Nothing in there.")
                    );
            return true;
        }
    }
}
