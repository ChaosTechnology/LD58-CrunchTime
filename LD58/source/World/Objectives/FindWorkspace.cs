using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using System;

namespace LD58.World.Objectives
{
    using Objects;
    using Objects.WorldObjects;
    using Interaction.Steps;
    using Player;

    class FindWorkspace
        : Objective
    {
        OfficeTable myDesk;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            for (Vector2i pos = 0; pos.x < scene.size.x; pos.x++)
                for (pos.y = 0; pos.y < scene.size.y; pos.y++)
                    if ((myDesk = scene[pos] as OfficeTable) != null)
                        if (myDesk.GetName() == "MyDesk")
                            goto ok;

            throw new Exception("Workplace not found. Too bad.");
        ok:

            myDesk.AddComponent<Highlight>(CreateParameters.Create(myDesk.bone));
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

        void Complete()
            => scene.SetObjective<DoWork>();
    }
}
