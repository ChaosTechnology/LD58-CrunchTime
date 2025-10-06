using ChaosFramework.Collections;
using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    using Interaction;
    using Interaction.Steps;
    using Inventory;
    using Player;

    public abstract class StockedInteractible
        : Interactible
    {
        protected readonly ItemBag stock = new ItemBag();

        protected virtual string displayName => name;

        public StockedInteractible(uint width = 1, uint height = 1)
            : base(width, height)
        { }

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            foreach (Item item in GetInitialStock())
                stock.Add(item);
        }

        protected abstract SysCol.IEnumerable<Item> GetInitialStock();

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            LinkedList<Choice.Option> choices = new LinkedList<Choice.Option>
            {
                EnumerateStockOptions(interactor),
                new Choice.Option("Leave")
            };

            interactor.AddInteraction(
                choices.length > 1
                ? new Choice(interactor, $"Take from {displayName}?", choices.ToArray())
                : new DialogLine(interactor, "There nothing in here.")
                );

            return true;
        }

        protected SysCol.IEnumerable<Choice.Option> EnumerateStockOptions(Interactor interactor)
        {
            foreach (System.Tuple<Item, int> _i in stock)
            {
                System.Tuple<Item, int> i = _i;
                yield return new Choice.Option(
                    $"{i.Item1.displayName} x{i.Item2}",
                    new InteractionStep[] {
                        new AddItem(interactor, i.Item1),
                        new CustomAction(interactor, () => stock.Remove(i.Item1))
                        }
                    );
            }
        }
    }
}
