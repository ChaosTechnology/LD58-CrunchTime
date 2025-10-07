using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using System;
using System.Linq;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Kitchen.gmdl", "objects/Kitchen.mat")]
    class Kitchen
        : Interactible
    {
        public Kitchen() : base(2, 1) { }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            if (IsStove(interactAt))
            {
                LinkedList<Tuple<Item, Item>> incredients = new LinkedList<Tuple<Item, Item>>();
                foreach (Tuple<Item, int> i in interactor.parent.inventory)
                    if (i.Item1.traits.HasFlag(Traits.Incredient))
                        if (i.Item1 == KnownItems.ESSENCE_OF_DARKNESS)
                            incredients.Add(new Tuple<Item, Item>(i.Item1, KnownItems.ETERNAL_DARKNESS));
                        else
                            incredients.Add(new Tuple<Item, Item>(
                                i.Item1,
                                new Item(
                                    $"Fried {i.Item1.displayName}",
                                    Traits.Food | i.Item1.traits & ~Traits.Incredient
                                    )
                                ));

                if (incredients.empty)
                    interactor.AddInteraction(
                        new DialogLine(interactor, "If I had some incredients, I could make breakfast here.")
                    );
                else
                {
                    LinkedList<Choice.Option> options = new LinkedList<Choice.Option>();
                    options.Add(new Choice.Option("Nah, I'm good."));
                    foreach (Tuple<Item, Item> _incredient in incredients)
                    {
                        Tuple<Item, Item> incredient = _incredient;
                        options.Add(new Choice.Option(
                            incredient.Item1.displayName,
                            new AddItem(interactor, incredient.Item2),
                            new CustomAction(interactor, (Interactor i) => i.parent.inventory.Remove(incredient.Item1))
                            ));
                    }

                    interactor.AddInteraction(new Choice(interactor, "Cook something?", options.ToArray()));
                }

                return true;
            }
            else
                return false;
        }

        public bool IsStove(Vector2i pos)
            => TransformRelativeTilePositions(new Vector2i(1, 0)).Contains(pos);
    }
}
