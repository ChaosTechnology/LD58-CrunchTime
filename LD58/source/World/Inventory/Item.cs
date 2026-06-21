namespace LD58.World.Inventory
{
    public class Item
    {
        public readonly string displayName;
        public readonly Traits traits;

        public Item(string displayName, Traits traits)
        {
            this.displayName = displayName;
            this.traits = traits;
        }

        public override int GetHashCode()
            => displayName.GetHashCode();

        public static bool operator ==(Item a, Item b)
            => a?.Equals(b) ?? (object)b == null;

        public static bool operator !=(Item a, Item b)
            => !(a == b);

        public override sealed bool Equals(object obj)
            => Equals(obj as Item);

        public virtual bool Equals(Item item)
            => (object)item != null && displayName == item.displayName && traits == item.traits;
    }
}
