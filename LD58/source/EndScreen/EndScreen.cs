using ChaosFramework.Components;
using LD58.World.Inventory;

namespace LD58.EndScreen
{
    internal class EndScreen : TextScene
    {
        public EndScreen(Game game, ItemBag collectedItems)
            : base(game)
        {
            AddComponent<CollectedCharacterTraits>(CreateParameters.Create(collectedItems));
        }
    }
}
