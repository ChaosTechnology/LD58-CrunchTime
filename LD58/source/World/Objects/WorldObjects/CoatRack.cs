using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Coat Rack.gmdl", "objects/Kitchen.mat")]
    class CoatRack
        : Interactible
    {
        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            interactor.AddInteraction(new DialogLine(interactor, "I wish I had some jackets..."));

            return true;
        }
    }
}
