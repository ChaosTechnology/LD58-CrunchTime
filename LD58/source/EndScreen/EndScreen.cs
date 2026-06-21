using ChaosFramework.Components;
using LD58.World.Inventory;
using LD58.World.Player;

namespace LD58.EndScreen
{
    internal class EndScreen : TextScene
    {
        public EndScreen(Game game, PlayerInventory collectedItems)
            : base(game)
        {
            AddComponent<CollectedCharacterTraits>(CreateParameters.Create(collectedItems));
        }
    }
}
