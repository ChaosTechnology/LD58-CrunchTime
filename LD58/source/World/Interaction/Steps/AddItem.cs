namespace LD58.World.Interaction.Steps
{
    using Inventory;
    using Player;

    class AddItem
        : InteractionStep
    {
        readonly Item item;

        public override bool interactionDone => true;

        public AddItem(Interactor interactor, Item item)
            : base(interactor)
        {
            this.item = item;
        }

        public override void Activate()
        {
            base.Activate();
            interactor.parent.inventory.AddItem(item);
        }
    }
}
