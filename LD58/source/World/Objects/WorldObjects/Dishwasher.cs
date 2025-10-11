using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Constants;
    using Inventory;

    [DefaultInstancer(64, "objects/Dishwasher.gmdl", "objects/Kitchen.mat")]
    class Dishwasher
        : StockedInteractible
    {
        protected override string prompt
            => "There are clean dishes in there.";

        protected override string takeOption
            => "Break!";

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            for (int i = 0; i < 4; ++i) yield return KnownItems.CLEAN_PLATE;
            for (int i = 0; i < 3; ++i) yield return KnownItems.CLEAN_BOWL;
            for (int i = 0; i < 6; ++i) yield return KnownItems.CLEAN_CUP;
            for (int i = 0; i < 3; ++i) yield return KnownItems.CLEAN_GLASS;
        }
    }
}
