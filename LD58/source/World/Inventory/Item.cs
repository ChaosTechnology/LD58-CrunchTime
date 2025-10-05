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
    }
}
