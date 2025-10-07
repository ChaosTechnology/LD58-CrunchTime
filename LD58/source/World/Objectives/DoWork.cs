using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class DoWork
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            myDesk = scene.Find<OfficeTable>("MyDesk");
            myDesk.AddComponent<Highlight>(CreateParameters.Create(myDesk.bone));
        }

        protected override string GetText()
            => "Do work!";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            if (interactible == myDesk && myDesk.FacingScreenZero(interactor.parent.direction))
            {
                interactor.AddInteraction(
                    new DialogLine(interactor, "Nah, let's skip this."),
                    new CustomAction(interactor, Complete)
                    );
                return true;
            }
            else
                return base.Interact(interactor, interactible, interactAt);
        }

        void Complete()
            => scene.SetObjective<LunchBreak>();
    }
}
