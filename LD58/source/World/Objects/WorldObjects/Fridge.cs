using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using LD58.World.Player;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction;
    using Interaction.Steps;
    using Inventory;

    [DefaultInstancer(64, "objects/Fridge.gmdl", "objects/Kitchen.mat")]
    class Fridge
        : Interactible
    {
        static readonly Item BEER = new Item("Beer", Traits.Beverage);
        static readonly Item EXTRA_MOLDY_CHEESE = new Item("Extra moldy cheese", Traits.Weird | Traits.Food);
        static readonly Item BLACK_SUBSTANCE = new Item("Unidentifiable black substance", Traits.Weird | Traits.Food | Traits.ClothingBottom);

        static readonly Item ESSENCE_OF_DARKNESS = new Item("Essence of darkness", Traits.Weird | Traits.Beverage);

        bool collectedEssenceOfDarkness = false;
        ItemBag stock = new ItemBag();

        public Fridge()
            : base(2, 1)
        {
            for (int i = 0; i < 3; ++i) stock.Add(EXTRA_MOLDY_CHEESE);
            for (int i = 0; i < 4; ++i) stock.Add(BEER);
            for (int i = 0; i < 2; ++i) stock.Add(BLACK_SUBSTANCE);
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            LinkedList<Choice.Option> choices = new LinkedList<Choice.Option>();
            foreach (System.Tuple<Item, int> _i in stock)
            {
                System.Tuple<Item, int> i = _i;
                choices.Add(new Choice.Option(
                    $"{i.Item1.displayName} x{i.Item2}",
                    new InteractionStep[] {
                        new AddItem(interactor, i.Item1),
                        new CustomAction(interactor, () => stock.Remove(i.Item1))
                        }
                    )
                );
            }

            if (!stock.Contains(BLACK_SUBSTANCE) && !collectedEssenceOfDarkness)
                choices.Add(
                    new Choice.Option(
                        ESSENCE_OF_DARKNESS.displayName,
                        new InteractionStep[] {
                            new AddItem(interactor, ESSENCE_OF_DARKNESS),
                            new CustomAction(interactor, CollectEssenceOfDarkness)
                            }
                        )
                    );

            LinkedList<InteractionStep> interactions = new LinkedList<InteractionStep>(
                new DialogLine(interactor, "Most of the food is spoiled."),
                new Choice(interactor, "Take some?", choices.ToArray())
                );

            if (choices.empty)
                interactions.Clear();

            if (!stock.Contains(BEER))
                interactions.Insert(0, new DialogLine(interactor, "Where has all the beer gone?"));

            interactor.AddInteraction(interactions);

            return true;
        }

        void CollectEssenceOfDarkness()
        {
            collectedEssenceOfDarkness = true;
        }
    }
}
