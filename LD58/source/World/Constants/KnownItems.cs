using LD58.World.Inventory;

namespace LD58.World.Constants
{
    internal static class KnownItems
    {
        public static readonly Item HELD_IN_POOP = new Item("Held-in Poop", Traits.Distressing | Traits.Invisible);
        public static readonly Item BODY_GREASE = new Item("Body grease", Traits.Weird | Traits.Invisible);
        public static readonly Item DIRTY_HANDS = new Item("Dirty hands", Traits.Weird | Traits.Invisible);

        public static readonly Item POOP = new Item("Poop", Traits.Weird | Traits.Food);

        public static readonly Item BEER = new Item("Beer", Traits.Beverage);
        public static readonly Item EXTRA_MOLDY_CHEESE = new Item("Extra moldy cheese", Traits.Weird | Traits.Food);
        public static readonly Item BLACK_SUBSTANCE = new Item("Unidentifiable black substance", Traits.Weird | Traits.Food | Traits.ClothingBottom);

        public static readonly Item ESSENCE_OF_DARKNESS = new Item("Essence of darkness", Traits.Weird | Traits.Beverage);

    }
}
