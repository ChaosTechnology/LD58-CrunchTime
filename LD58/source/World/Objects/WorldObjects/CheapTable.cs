using ChaosFramework.Math.Vectors;
using LD58.World.Player;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Cheap Table.gmdl", "objects/Office.mat")]
    public class CheapTable
        : Interactible
    {
        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => false;
    }
}
