namespace LD58.World.Interaction.Steps
{
    using Player;

    class CustomAction
        : InteractionStep
    {
        readonly System.Action action;

        public override bool interactionDone => true;

        public CustomAction(Interactor interactor, System.Action action)
            : base(interactor)
        {
            this.action = action;
        }

        public override void Activate()
        {
            base.Activate();
            action();
        }
    }
}
