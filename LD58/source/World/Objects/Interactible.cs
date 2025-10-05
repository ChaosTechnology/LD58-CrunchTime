namespace LD58.World.Objects
{
    using Interaction;

    public abstract class Interactible
        : WorldObject
    {
        public abstract bool Interact(Interactor interactor);
    }
}
