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
        static bool FeetCovered(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelection)
            => traitsFromSelection.ContainsKey(Traits.CoversFeet);

        static bool BottomCovered(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelection)
            => traitsFromSelection.ContainsKey(Traits.CoversBottom);

        static bool TopCovered(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelection)
            => traitsFromSelection.ContainsKey(Traits.CoversTop);

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            Wardrobe wardrobe = interactible as Wardrobe;
            if (wardrobe != null)
            {
                interactor.AddInteraction(new Choice(interactor, "I can get dressed here...",
                    new Choice.Option("Rummage through this wardrobe...",
                        new CustomAction(interactor, (Interactor i) => interactible.Interact(interactor, interactAt))
                        ),
                    new Choice.Option("Choose clothes...", new CustomAction(interactor, (Interactor i) =>
                        i.AddInteraction(new TransformItemsDialog(
                            i,
                            "Choose clothes to wear:",
                            "Wear this!",
                            Traits.Clothing,
                            Complete,
                            new TransformItemsDialog.Requirement("Need some bottom clothing piece.", BottomCovered),
                            new TransformItemsDialog.Requirement("Can't go topless.", TopCovered),
                            new TransformItemsDialog.Requirement("I still need something for the feet.", FeetCovered)
                            ))
                        )),
                    new Choice.Option("Leave")
                    ));
                return true;
            }

            return interactible.Interact(interactor, interactAt);
        }

        void Complete(Interactor interactor, SysCol.Dictionary<Item, int> selectedItems)
        {
            foreach (SysCol.KeyValuePair<Item, int> item in selectedItems)
                for (int i = 0; i < item.Value; ++i)
                {
                    interactor.parent.inventory.Remove(item.Key);
                    interactor.parent.inventory.AddItem(new Item(
                        $"Wearing {item.Key.displayName}",
                        item.Key.traits | Traits.Wearing | Traits.Invisible
                        ));
                }

            interactor.parent.scene.SetObjective<PrepareBreakfast>();
        }

        protected override string GetText()
            => "Get dressed";
    }
}
