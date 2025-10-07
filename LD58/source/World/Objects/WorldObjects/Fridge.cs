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
        bool collectedEssenceOfDarkness = false;

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

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            LinkedList<Choice.Option> choices = new LinkedList<Choice.Option>(EnumerateStockOptions(interactor));

            if (scene.name == "home" && !stock.Contains(KnownItems.BLACK_SUBSTANCE) && !collectedEssenceOfDarkness)
                choices.Add(
                    new Choice.Option(
                        KnownItems.ESSENCE_OF_DARKNESS.displayName,
                        new InteractionStep[] {
                            new AddItem(interactor, KnownItems.ESSENCE_OF_DARKNESS),
                            new CustomAction(interactor, CollectEssenceOfDarkness)
                            }
                        )
                    );

            LinkedList<InteractionStep> interactions = new LinkedList<InteractionStep>();
            if (choices.empty)
                if (scene.name == "home" && !stock.Contains(KnownItems.BEER))
                    interactions.Insert(0, new DialogLine(interactor, "Where has all the beer gone?"));
                else
                    interactions.Insert(0, new DialogLine(interactor, "It's empty."));
            else
                switch (scene.name)
                {
                    case "home":
                        interactions.Add(
                            new DialogLine(interactor, "Most of the food is spoiled."),
                            new Choice(interactor, "Take some?", choices.ToArray())
                            );
                        break;

                    case "office":
                        interactions.Add(
                            new DialogLine(interactor, "I've never put anything in there."),
                            new DialogLine(interactor, "My coworkers may have done that though..."),
                            new Choice(interactor, "Nobody's here.\nSteal some?", choices.ToArray())
                            );
                        break;
                }

            interactor.AddInteraction(interactions);
            return true;
        }

        void CollectEssenceOfDarkness(Interactor interactor)
            => collectedEssenceOfDarkness = true;
    }
}
