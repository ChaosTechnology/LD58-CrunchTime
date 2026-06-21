namespace LD58.World.Interaction.Steps
{
    using Player;

    class CustomAction
        : InteractionStep
    {
        readonly System.Action<Interactor> action;

        public override bool interactionDone => true;

        public CustomAction(Interactor interactor, System.Action<Interactor> action)
            : base(interactor)
        {
            this.action = action;
        }

        public override void Activate()
        {
            base.Activate();
            action(interactor);
        }
    }
}
