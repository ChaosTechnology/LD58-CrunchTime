using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Inventory;

    [DefaultInstancer(64, "objects/Chair.gmdl", "objects/Kitchen.mat")]
    class Chair
        : StockedInteractible
    {
        protected override string displayName => "chair";

        static readonly SysCol.Dictionary<string, Item[]> stocksByName = new SysCol.Dictionary<string, Item[]>()
        {
            ["Bedroom"] = new   [] {
                KnownItems.KEY_CHAIN,
            },
        };

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            Item[] a;
            return stocksByName.TryGetValue(name ?? "", out a) ? a : Array<Item>.empty;
        }
    }
}
