using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Inventory;
    using Objects.WorldObjects;
    using Player;
    using Constants;

    class LunchBreak
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            myDesk = scene.Find<OfficeTable>("MyDesk");

            foreach (Player p in scene.EnumerateChildren<Player>(false))
            {
                p.inventory.AddItem(KnownItems.HELD_IN_POOP);
                foreach (System.Tuple<Item, int> i in p.inventory)
                    if (i.Item1.traits.HasFlag(Traits.Consumed))
                        p.inventory.Remove(i.Item1, true);
            }
        }

        protected override string GetText()
            => "Take a break.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            if (interactible == myDesk && myDesk.FacingScreenZero(interactor.parent.direction))
            {
                interactor.AddInteraction(
                    new DialogLine(interactor, "Need to take a break first.")
                    );
                return true;
            }
            else
                return base.Interact(interactor, interactible, interactAt);
        }
    }
}
