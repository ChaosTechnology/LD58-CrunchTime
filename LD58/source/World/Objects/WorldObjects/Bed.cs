using ChaosFramework.Math.Vectors;
using LD58.World.Player;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Bed.gmdl", "objects/Bed.mat")]
    class Bed
        : Interactible
    {
        public Bed() : base(2, 3) { }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => false;
    }
}
