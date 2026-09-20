namespace LD58.World.Objects.WorldObjects
{
    using System.Collections.Generic;
    using Constants;
    using Inventory;

    [DefaultInstancer(64, "objects/Wardrobe.gmdl", "objects/Kitchen.mat")]
    class Wardrobe
        : StockedInteractible
    {
        static readonly Dictionary<string, Item[]> stocksByName = new Dictionary<string, Item[]>()
        {
            ["Bedroom 1"] = new[] {
                KnownItems.BLACK_SOCKS,
                KnownItems.BLACK_SOCKS,
                KnownItems.PROGRAMMER_SOCKS,
                KnownItems.PANTS,
                KnownItems.PANTS,
            },

            ["Bedroom 2"] = new[] {
                KnownItems.BLACK_SOCKS,
                KnownItems.PANTS,
                KnownItems.DENIM_TROUSERS,
                KnownItems.FUR_SUIT,
            },

            ["Hallway"] = new[] {
                KnownItems.DENIM_TROUSERS,
                KnownItems.DENIM_TROUSERS,
                KnownItems.DENIM_TROUSERS,
                KnownItems.TSHIRT,
            },

            ["Secret Room A"] = new[] {
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.SHOT_GLASS,
                KnownItems.BEER_JAR,
                KnownItems.BEER_JAR,
                KnownItems.BEER_JAR,
                KnownItems.BEER_JAR,
                KnownItems.BEER_JAR,
                KnownItems.STALE_BEER,
                KnownItems.BAG_OF_CHIPS,
                KnownItems.BAG_OF_CHIPS,
                KnownItems.MOLTEN_ICE_CUBES,
                KnownItems.MOLTEN_ICE_CUBES,
                KnownItems.MOLTEN_ICE_CUBES,
            },

            ["Secret Room B"] = new[] {
                KnownItems.DILDO,
                KnownItems.LUBE,
                KnownItems.LUBE,
                KnownItems.NIPPLE_CLAMPS,
                KnownItems.CANDY_UNDIES,
                KnownItems.CANDY_UNDIES,
                KnownItems.CANDY_UNDIES,
            }
        };

        protected override string displayName => "Wardrobe";

        public Wardrobe() : base(2, 1) { }

        protected override IEnumerable<Item> GetInitialStock()
        {
            Item[] stock;
            if (!string.IsNullOrEmpty(name) && stocksByName.TryGetValue(name, out stock))
                return stock;
            else
                return ChaosUtil.Primitives.Array<Item>.empty;
        }
    }
}
