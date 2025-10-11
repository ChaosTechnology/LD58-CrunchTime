using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Interaction;
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Fridge.gmdl", "objects/Kitchen.mat")]
    class Fridge
        : StockedInteractible
    {
        bool stockedEssenceOfDarkness = false;

        protected override string promptEmpty
            => scene.name == "home" && !stock.Contains(KnownItems.BEER)
                ? "Where has all the beer gone?"
                : "It's empty."
                ;


        public Fridge()
            : base(2, 1)
        { }

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            switch (scene.name)
            {
                case "home":
                    for (int i = 0; i < 2; ++i) yield return KnownItems.EGG;
                    for (int i = 0; i < 1; ++i) yield return KnownItems.BACON;
                    for (int i = 0; i < 3; ++i) yield return KnownItems.EXTRA_MOLDY_CHEESE;
                    for (int i = 0; i < 4; ++i) yield return KnownItems.BEER;
                    for (int i = 0; i < 2; ++i) yield return KnownItems.BLACK_SUBSTANCE;
                    break;

                case "office":
                    for (int i = 0; i < 2; ++i) yield return KnownItems.DONUT;
                    for (int i = 0; i < 2; ++i) yield return KnownItems.COLD_COFFEE;
                    for (int i = 0; i < 7; ++i) yield return KnownItems.SOFT_DRINK;
                    break;
            }
        }

        protected override SysCol.IEnumerable<InteractionStep> PrependSteps(Interactor interactor)
        {
            switch (scene.name)
            {
                case "home":
                    yield return new DialogLine(interactor, "Most of the food is spoiled.");
                    break;

                case "office":
                    yield return new DialogLine(interactor, "I've never put anything in there.");
                    yield return new DialogLine(interactor, "My coworkers may have done that though...");
                    break;
            }
        }

        protected override void SuccessCallback(Interactor interactor, ItemBag selectedItems)
        {
            base.SuccessCallback(interactor, selectedItems);
            if (!stockedEssenceOfDarkness && !stock.Contains(KnownItems.BLACK_SUBSTANCE))
            {
                stock.Add(KnownItems.ESSENCE_OF_DARKNESS);
                stockedEssenceOfDarkness = true;
            }
        }
    }
}
