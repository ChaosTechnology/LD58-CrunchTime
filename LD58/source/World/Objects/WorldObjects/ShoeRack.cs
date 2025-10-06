using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Inventory;

    [DefaultInstancer(64, "objects/Shoe Rack.gmdl", "objects/Kitchen.mat")]
    class ShoeRack
        : StockedInteractible
    {
        public ShoeRack() : base(2, 1) { }

        protected override string displayName => "shoe rack";

        static readonly SysCol.Dictionary<string, Item[]> stocksByName = new SysCol.Dictionary<string, Item[]>()
        {
            ["Apartment"] = new[] {
                KnownItems.CROWBAR,
            },
        };

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            Item[] a;
            return stocksByName.TryGetValue(name, out a) ? a : Array<Item>.empty;
        }
    }
}
