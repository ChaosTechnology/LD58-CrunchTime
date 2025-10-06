namespace LD58.World.Inventory
{
    [System.Flags]
    public enum Traits
        : ushort
    {
        Weird = 1,
        Food = 1 << 1,
        Beverage = 1 << 2,
        ClothingTop = 1 << 3,
        ClothingBottom = 1 << 4,
        ClothingFeet = 1 << 5,
    }
}
