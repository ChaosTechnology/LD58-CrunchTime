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
            }
        };

        protected override string displayName => "Wardrobe";

        public Wardrobe() : base(2, 1) { }

        protected override IEnumerable<Item> GetInitialStock()
            => stocksByName[name];
    }
}
