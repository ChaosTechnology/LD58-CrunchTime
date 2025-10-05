namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Table.gmdl", "objects/Kitchen.mat")]
    class Table
        : WorldObject
    {
        public Table() : base(2, 2) { }
    }
}
