using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Inventory;
    [DefaultInstancer(64, "objects/Long Counter.gmdl", "objects/Kitchen.mat")]
    class LongCounter
        : StockedInteractible
    {
        public LongCounter() : base(4, 1) { }

        protected override string displayName => "this unnecessarily long counter";

        static readonly SysCol.Dictionary<string, Item[]> stocksByName = new SysCol.Dictionary<string, Item[]>()
        {
            ["Apartment"] = new[] {
                KnownItems.SHORTWIRE,
            },
            ["Minibar"] = new[] {
                KnownItems.VODKA,
                KnownItems.SMOKY_IRISH_WHISKEY,
                KnownItems.SMOKY_IRISH_WHISKEY,
                KnownItems.SCOTCH,
                KnownItems.TONIC_WATER,
                KnownItems.TONIC_WATER,
                KnownItems.TONIC_WATER,
                KnownItems.GIN,
            },
        };

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            Item[] a;
            return stocksByName.TryGetValue(name ?? "", out a) ? a : Array<Item>.empty;
        }
    }
}
