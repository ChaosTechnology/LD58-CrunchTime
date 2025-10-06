namespace LD58.World.Inventory
{
    [System.Flags]
    public enum Traits
        : ushort
    {
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
        ClothingTop = Clothing | CoversTop,
        ClothingBottom = Clothing | CoversBottom,
        ClothingFeet = Clothing | CoversFeet,
    }
}
