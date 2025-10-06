namespace LD58.World.Objects
{
    using Interaction;

    public abstract class Interactible
        : WorldObject
    {
        public Interactible() { }

        public Interactible(uint width, uint height)
            : base(width, height)
        { }

        public abstract bool Interact(Interactor interactor);
    }
}
