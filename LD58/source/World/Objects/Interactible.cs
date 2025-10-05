namespace LD58.World.Objects
{
    using Player;

    public abstract class Interactible
        : WorldObject
    {
        public Interactible(uint width = 1, uint height = 1)
            : base(width, height)
        { }

        public abstract bool Interact(Interactor interactor);
    }
}
