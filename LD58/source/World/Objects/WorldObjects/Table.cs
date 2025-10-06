using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    using Player;

    [DefaultInstancer(64, "objects/Table.gmdl", "objects/Kitchen.mat")]
    class Table
        : Interactible
    {
        public Table() : base(2, 2) { }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => false;
    }
}
