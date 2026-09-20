using System.Linq;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Player;
    using World.Objects.WorldObjects;

    class ConsiderDoWork2
        : Objective
    {
        bool motivated = true;

        protected override string GetText()
            => DoWork.DO_WORK;

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            if (motivated)
                scene.updateLayers[(int)UpdateLayers.ObjectiveLogic].Add(ConsiderWork);
        }

        void ConsiderWork()
        {
            motivated = false;

            if (scene.EnumerateChildren<WorkItem>(true).Any())
                foreach (Interactor interactor in scene.EnumerateChildren<Interactor>(true))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Oh right, everything broke down earlier.\nThere's no way I can fix all that."),
                        new DialogLine(interactor, "With the servers down, this company will be bankrupt within hours..."),
                        new DialogLine(interactor, "Hmm... looks like I'll be out of a job for a while then."),
                        new DialogLine(interactor, "Well..."),
                        new DialogLine(interactor, "..."),
                        new DialogLine(interactor, "What a depressing thought."),
                        new DialogLine(interactor, "..."),
                        new DialogLine(interactor, "The others really could have pulled their weight, though!"),
                        new DialogLine(interactor, "I just can't do everyone's work."),
                        new DialogLine(interactor, "..."),
                        new DialogLine(interactor, "I need to rest."),
                        new CustomAction(interactor, GoHome)
                        );
            else
                foreach (Interactor interactor in scene.EnumerateChildren<Interactor>(true))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Let's get on with it..."),
                        new CustomAction(interactor, DoWork2)
                        );
        }

        void GoHome(Interactor _)
        {
            scene.SetObjective<GoHome>();
            scene.Find<DoorFrame>("Exit").Unlock();
        }

        void DoWork2(Interactor _)
            => scene.SetObjective<DoWork2>();
    }
}
