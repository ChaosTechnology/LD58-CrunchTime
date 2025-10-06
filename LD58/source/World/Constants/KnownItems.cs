using LD58.World.Inventory;

namespace LD58.World.Constants
{
    internal static class KnownItems
    {
        public static readonly Item HELD_IN_POOP = new Item("Held-in Poop", Traits.Distressing);
        public static readonly Item POOP = new Item("Poop", Traits.Weird | Traits.Food);
    }
}
