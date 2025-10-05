namespace LD58.World.Objects
{
    public abstract class Interactible
        : WorldObject
    {
        public abstract bool Interact(Player player);
    }
}
