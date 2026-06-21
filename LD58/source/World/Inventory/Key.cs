namespace LD58.World.Inventory
{
    class Key
        : Item
    {
        public readonly string doorName;

        public Key(string displayName, string doorName)
            : base(displayName, Traits.OpensDoor)
        {
            this.doorName = doorName;
        }

        public override bool Equals(Item item)
            => Equals(item as Key);

        public bool Equals(Key key)
            => base.Equals(key) && doorName == key.doorName;
    }
}
