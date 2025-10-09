using ChaosFramework.Components;
using System.Linq;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects.WorldObjects;
    using Player;

    class DoWork2
        : DoWork
    {
        static readonly string[] SUPER_LAZY_THOUGHTS = new[] {
            "No, I need to get started now.",
            "It's not very fun though, all alone.",
            "I really, really don't want to do this.",
            "...",
            "Who cares for this anyways?",
            "...",
            "...",
            "...",
            "...",
            "...",
            "...",
            "...",
            "I'm leaving. This is pointless anyways.",
        };

        static readonly string[] LAZY_THOUGHTS = new[] {
            "I've already started, gotta commit now!",
            "There's still so much to do...",
            "I'm already so burnt't out...",
            "I really wanna go home, but I really can't yet.",
            "I should get back to work...",
        };

        static readonly string[] ALMOST_DONE_THOUGHTS = new[] {
            "almost done, then I can go home!",
            "Just a bit more to do!",
            "...",
            "... I need to get this wrapped up.",
        };

        static readonly string[] DONE_THOUGHTS = new[] {
            "I've gotten enough done for today.",
            "Hmm... maybe I can do one or two more things",
            "I'm still pretty motivated, but I feel quite tired.",
            "Eh, whatever, I'm calling it quits!",
        };

        static readonly string SUPER_DONE_THOUGHT = "That was an insanely productive day! Time to go home.";

        protected override int required => 25;
        protected override string[] superLazyThoughts => SUPER_LAZY_THOUGHTS;
        protected override string[] lazyThoughts => LAZY_THOUGHTS;
        protected override string[] almostDoneThoughts => ALMOST_DONE_THOUGHTS;
        protected override string[] doneThoughts => DONE_THOUGHTS;
        protected override string superDoneThought => SUPER_DONE_THOUGHT;
        protected override string failureThought => "All I can do is go home now...";
        protected override string endOption => "Wrap up";
        protected override string endThought => "That's enough for the day. Time to wrap up.";

        DoorFrame exitDoor;

        bool failedPreviously = false;
        bool ex = false;

        protected override void Create(CreateParameters cparams)
        {
            if (scene.EnumerateChildren<WorkItem>(true).Any())
            {
                failedPreviously = true;
                return;
            }

            base.Create(cparams);
            objectiveEndingDoors.Add(exitDoor = scene.Find<DoorFrame>("Exit"));
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            scene.updateLayers[(int)UpdateLayers.ObjectiveLogic].Add(SkipIfFailed);
        }

        void SkipIfFailed()
        {
            if (ex)
                return;

            ex = true;
            foreach (Interactor interactor in scene.EnumerateChildren<Interactor>(false))
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
                    new CustomAction(interactor, ConcludeFailed)
                    );

        }

        protected override void Complete(Interactor _)
        {
            ConcludeWork(!failure);
            scene.SetObjective<GoHome>();
        }

        void ConcludeFailed(Interactor _)
        {
            ConcludeWork(false);
        }
    }
}