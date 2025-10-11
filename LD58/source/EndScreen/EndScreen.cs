using ChaosFramework.Components;
using ChaosFramework.Graphics;

namespace LD58.EndScreen
{
    internal class EndScreen : TextScene
    {
        public Camera fullScreenView { get; set; }

        public EndScreen(Game game)
            : base(game)
        {
            AddComponent<CollectedCharacterTraits>();
        }
    }
}
