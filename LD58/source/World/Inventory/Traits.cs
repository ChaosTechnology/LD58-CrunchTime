namespace LD58.World.Inventory
{
    [System.Flags]
    public enum Traits
        : uint
    {
        None = 0,
        Invisible = 1,
        Weird = 1 << 1,
        Distressing = 1 << 2,
        RESERVED_EMOTION_2 = 1 << 3,
        RESERVED_EMOTION_3 = 1 << 4,
        RESERVED_EMOTION_4 = 1 << 5,
        Food = 1 << 6,
        Beverage = 1 << 7,
        Clothing = 1 << 8,
        CoversTop = 1 << 9,
        CoversBottom = 1 << 10,
        CoversFeet = 1 << 11,
        Incredient = 1 << 12,
        Dish = 1 << 13,
        StartsCar = 1 << 14,
        OpensDoor = 1 << 15,
        Consumed = 1 << 16,
        Wearing = 1 << 17,
        Caffeine = 1 << 18,
        Alcohol = 1 << 19,

        ClothingTop = Clothing | CoversTop,
        ClothingBottom = Clothing | CoversBottom,
        ClothingFeet = Clothing | CoversFeet,
        WellFed = Food | Consumed,
        Hydrated = Beverage | Consumed,
        Caffeinated = Caffeine | Consumed,
        Drunk = Alcohol | Consumed
    }
}
