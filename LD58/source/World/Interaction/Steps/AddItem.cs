namespace LD58.World.Interaction.Steps
{
    using Inventory;
    using Player;

    class AddItem
        : InteractionStep
    {
        readonly Item item;

        bool done = false;

        public AddItem(Interactor interactor, Item item)
            : base(interactor)
        {
            this.item = item;
        }

        public override bool interactionDone => done;

        public override void SetDrawCalls() { }

        public override void SetUpdateCalls()
            => interactor.scene.updateLayers[(int)UpdateLayers.PrepareInteraction].Add(Perform);

        void Perform()
        {
            interactor.parent.inventory.AddItem(item);
            done = true;
        }
    }
}
