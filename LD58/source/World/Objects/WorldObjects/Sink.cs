using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using System.Linq;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Sink.gmdl", "objects/Bathroom.mat")]
    class Sink
        : Interactible
    {
        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            PlayerInventory inventory = interactor.parent.inventory;
            LinkedList<Choice.Option> options = new LinkedList<Choice.Option>();

            if (inventory.Contains(KnownItems.DIRTY_HANDS))
                options.Add(new Choice.Option(
                            "Wash hands",
                            new CustomAction(interactor, (Interactor _) => inventory.Remove(KnownItems.DIRTY_HANDS, all: true))
                            ));

            SysCol.IEnumerable<System.Tuple<Item, int>> dirtyDishes
                = inventory.Where(item => item.Item1.traits.HasFlag(Traits.Dish | Traits.Consumed));

            if (dirtyDishes.Any())
                options.Add(new Choice.Option(
                            "Clean dishes",
                            new CustomAction(interactor, (Interactor _) =>
                            {
                                foreach (System.Tuple<Item, int> dish in dirtyDishes)
                                {
                                    inventory.Remove(dish.Item1, all: true);
                                    for (int i = 0; i < dish.Item2; i++)
                                        inventory.AddItem(new Item(dish.Item1.displayName, dish.Item1.traits & ~(Traits.Consumed | Traits.Invisible)));
                                }
                            })
                            ));

            options.Add(new Choice.Option("Leave"));

            interactor.AddInteraction(
                options.length == 1
                ? new DialogLine(interactor, "My hands are clean.")
                : new Choice(interactor, "What to do...", options.ToArray())
                );

            return true;
        }
    }
}
