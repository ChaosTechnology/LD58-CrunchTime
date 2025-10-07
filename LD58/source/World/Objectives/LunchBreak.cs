using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class LunchBreak
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            myDesk = scene.Find<OfficeTable>("MyDesk");
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
