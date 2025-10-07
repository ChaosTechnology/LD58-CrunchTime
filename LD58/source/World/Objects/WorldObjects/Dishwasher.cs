using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

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
        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            for (int i = 0; i < 4; ++i) yield return KnownItems.CLEAN_PLATE;
            for (int i = 0; i < 3; ++i) yield return KnownItems.CLEAN_BOWL;
            for (int i = 0; i < 6; ++i) yield return KnownItems.CLEAN_CUP;
            for (int i = 0; i < 3; ++i) yield return KnownItems.CLEAN_GLASS;
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            LinkedList<Choice.Option> options = new LinkedList<Choice.Option>(EnumerateStockOptions(interactor));
            options.Add(new Choice.Option("Leave."));

            if (stock.Contains(KnownItems.CLEAN_PLATE))
                interactor.AddInteraction(
                    new Choice(
                        interactor,
                        "Take dishes?",
                        options.ToArray()
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
