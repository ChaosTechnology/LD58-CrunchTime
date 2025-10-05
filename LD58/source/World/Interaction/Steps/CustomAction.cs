namespace LD58.World.Interaction.Steps
{
    using Player;

    class CustomAction
        : InteractionStep
    {
        readonly System.Action action;

        bool done = false;

        public CustomAction(Interactor interactor, System.Action action)
            : base(interactor)
        {
            this.action = action;
        }

        public override bool interactionDone => done;

        public override void SetDrawCalls() { }

        public override void SetUpdateCalls()
            => interactor.scene.updateLayers[(int)UpdateLayers.PrepareInteraction].Add(Perform);

        void Perform()
        {
            action();
            done = true;
        }
    }
}
