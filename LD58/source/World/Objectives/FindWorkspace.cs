using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class FindWorkspace
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            myDesk = scene.Find<OfficeTable>("MyDesk");
            scene.Find<DoorFrame>("Exit").Lock();
            scene.Find<DoorFrame>("Boss Office").Lock();
            scene.Find<DoorFrameSmall>("Secret Room").Lock();
        }

        protected override string GetText()
            => "Find your desk.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            if (interactible == myDesk && myDesk.FacingScreenZero(interactor.parent.direction))
            {
                interactor.AddInteraction(
                    new DialogLine(interactor, "Damn, this code sucks."),
                    new DialogLine(interactor, "Who the hell wrote this."),
                    new DialogLine(interactor, "I really don't want to do this."),
                    new DialogLine(interactor, "Can I just dump this on some coworker?"),
                    new DialogLine(interactor, "Where are my coworkers anyways?"),
                    new DialogLine(interactor, "Are they seriously dumping all their work on me?"),
                    new DialogLine(interactor, "..."),
                    new DialogLine(interactor, "Better get on with it..."),
                    new CustomAction(interactor, Complete)
                    );
                return true;
            }
            else
                return base.Interact(interactor, interactible, interactAt);
        }

        void Complete(Interactor interactor)
            => scene.SetObjective<DoWork>();
    }
}
