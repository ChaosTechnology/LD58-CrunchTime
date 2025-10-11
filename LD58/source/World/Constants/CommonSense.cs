using SysCol = System.Collections.Generic;

namespace LD58.World.Constants
{
    using Inventory;
    using LD58.World.Interaction.Steps;
    using Player;

    public static class CommonSense
    {
        public static readonly TransformItemsDialog.Requirement FOOD_NEEDS_DISH = new TransformItemsDialog.Requirement(
                "I need a dish to eat this from.",
                HasPlateForFoods
            );

        public static readonly TransformItemsDialog.Requirement BEVERAGE_NEEDS_CONTAINER = new TransformItemsDialog.Requirement(
                "Can't just pour my drink on the table.",
                HasContainerForBeverage
            );

        static bool HasPlateForFoods(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelected)
            => !traitsFromSelected.ContainsKey(Traits.RequiresDish) || traitsFromSelected.ContainsKey(Traits.Dish);

        static bool HasContainerForBeverage(Interactor interactor, ItemBag selected, SysCol.Dictionary<Traits, int> traitsFromSelected)
            => !traitsFromSelected.ContainsKey(Traits.RequiresLiquidContainer) || traitsFromSelected.ContainsKey(Traits.LiquidContainer);
    }
}
