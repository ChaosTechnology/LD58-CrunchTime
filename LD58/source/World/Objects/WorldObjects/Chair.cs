using ChaosFramework.Collections;
using ChaosUtil.Primitives;
using System.Linq;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using ChaosFramework.Math.Vectors;
    using Constants;
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Chair.gmdl", "objects/Kitchen.mat")]
    class Chair
        : StockedInteractible
    {
        protected override string displayName => "chair";

        static readonly SysCol.Dictionary<string, Item[]> stocksByName = new SysCol.Dictionary<string, Item[]>()
        {
            ["Bedroom"] = new[] {
                KnownItems.KEY_CHAIN,
            },
        };

        protected override SysCol.IEnumerable<Item> GetInitialStock()
        {
            Item[] a;
            return stocksByName.TryGetValue(name ?? "", out a) ? a : Array<Item>.empty;
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => EnumerateStockOptions(interactor).Any(Linq.PredicateTrue<Choice.Option>)
            && base.Interact(interactor, interactAt);
    }
}
