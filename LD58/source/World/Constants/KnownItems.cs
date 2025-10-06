using LD58.World.Inventory;

namespace LD58.World.Constants
{
    internal static class KnownItems
    {
        // Hygiene
        public static readonly Item HELD_IN_POOP = new Item("Held-in Poop", Traits.Distressing | Traits.Invisible);
        public static readonly Item BODY_GREASE = new Item("Body grease", Traits.Weird | Traits.Invisible);
        public static readonly Item DIRTY_HANDS = new Item("Dirty hands", Traits.Weird | Traits.Invisible);

        public static readonly Item POOP = new Item("Poop", Traits.Weird | Traits.Food);

        // Kitchen
        public static readonly Item BEER = new Item("Beer", Traits.Beverage);
        public static readonly Item EXTRA_MOLDY_CHEESE = new Item("Extra moldy cheese", Traits.Weird | Traits.Food);
        public static readonly Item BLACK_SUBSTANCE = new Item("Unidentifiable black substance", Traits.Weird | Traits.Food | Traits.ClothingBottom);

        public static readonly Item ESSENCE_OF_DARKNESS = new Item("Essence of darkness", Traits.Weird | Traits.Beverage);

        // Clothing
        public static readonly Item PANTS = new Item("Pants", Traits.ClothingBottom);
        public static readonly Item DENIM_TROUSERS = new Item("Denims", Traits.ClothingBottom);
        public static readonly Item SCOTTISH_DRESS = new Item("Scottish Dress", Traits.ClothingBottom);
        public static readonly Item BLACK_SOCKS = new Item("Black Socks", Traits.ClothingFeet);
        public static readonly Item TSHIRT = new Item("T-Shirt", Traits.ClothingTop);
        public static readonly Item PULLOVER = new Item("Pullover", Traits.ClothingTop);

        public static readonly Item PROGRAMMER_SOCKS = new Item("Programmer Socks", Traits.ClothingFeet | Traits.Weird);
        public static readonly Item FUR_SUIT = new Item("Fur suit", Traits.ClothingFeet | Traits.ClothingBottom | Traits.ClothingTop | Traits.Weird);
        public static readonly Item GARBAGE_BAG = new Item("Garbage bag", Traits.ClothingTop | Traits.ClothingBottom | Traits.Weird);

    }
}
