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

        public static readonly Item EGG = new Item("Egg", Traits.Incredient);
        public static readonly Item BACON = new Item("Bacon", Traits.Incredient);

        public static readonly Item ESSENCE_OF_DARKNESS = new Item("Essence of darkness", Traits.Weird | Traits.Beverage | Traits.Incredient);

        public static readonly Item ETERNAL_DARKNESS = new Item("Eternal darkness", Traits.Weird | Traits.Beverage | Traits.Food);

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

        // Get to work
        public static readonly Item CROWBAR = new Item("Crowbar", Traits.OpensDoor);
        public static readonly Item SHORTWIRE = new Item("Shortwire", Traits.StartsCar);
        public static readonly Item KEY_CHAIN = new Item("Key chain", Traits.OpensDoor | Traits.StartsCar);

        public static readonly Item CLEAN_PLATE = new Item("Plate", Traits.Dish);

        // Lunch break
        public static readonly Item DONUT = new Item("Donut", Traits.Food);
        public static readonly Item COFFEE_BEANS = new Item("Coffee Beans", Traits.Incredient | Traits.Food | Traits.Caffeine);
        public static readonly Item COFFEE = new Item("Coffee", Traits.Incredient | Traits.Beverage | Traits.Caffeine);
        public static readonly Item COLD_COFFEE = new Item("Cold Coffee", Traits.Beverage | Traits.Caffeine);
        public static readonly Item SOFT_DRINK = new Item("Soft drink", Traits.Beverage);
        public static readonly Item ENERGY_DRINK = new Item("Energy drink", Traits.Beverage | Traits.Caffeine);
        public static readonly Item SMOKY_IRISH_WHISKEY = new Item("Smoky Irish Whiskey", Traits.Beverage | Traits.Alcohol);
        public static readonly Item SCOTCH = new Item("Scotch", Traits.Beverage | Traits.Alcohol);
        public static readonly Item GIN = new Item("London Dry Gin", Traits.Beverage | Traits.Alcohol);
        public static readonly Item TONIC_WATER = new Item("Tonic Water", Traits.Beverage);
        public static readonly Item VODKA = new Item("Vodka", Traits.Beverage | Traits.Alcohol);
        public static readonly Item SECRET_ROOM_KEY = new Item("Suspicious Key", Traits.OpensDoor);
    }
}
