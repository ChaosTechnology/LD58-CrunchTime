using ChaosFramework.Components;

namespace LD58.World.Objectives
{
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
        protected override string optionalGoal => "Go home.";
        protected override string overtimeFinishGoal => "Go home already!";

        DoorFrame exitDoor;

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            objectiveEndingDoors.Add(exitDoor = scene.Find<DoorFrame>("Exit"));
        }

        protected override void Complete(Interactor interactor)
        {
            ConcludeWork(!failure);
            GoHome(interactor);
        }

        void ConcludeFailed(Interactor _)
            => ConcludeWork(false);

        void GoHome(Interactor _)
            => scene.SetObjective<GoHome>();
    }
}
