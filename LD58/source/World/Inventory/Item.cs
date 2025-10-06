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
            => a.displayName == b.displayName && a.traits == b.traits;

        public static bool operator !=(Item a, Item b)
            => !(a == b);

        public override bool Equals(object obj)
            => Equals(obj as Item);

        public bool Equals(Item item)
            => this == item;
    }
}
