namespace LD58.World.Inventory
{
    [System.Flags]
    public enum Traits
        : ushort
    {
        Weird = 1,
        Distressing = 1 << 1,
        RESERVED_EMOTION_1 = 1 << 2,
        RESERVED_EMOTION_2 = 1 << 3,
        RESERVED_EMOTION_3 = 1 << 4,
        RESERVED_EMOTION_4 = 1 << 5,
        Food = 1 << 6,
        Beverage = 1 << 7,
        ClothingTop = 1 << 8,
        ClothingBottom = 1 << 9,
        ClothingFeet = 1 << 10,
    }
}
