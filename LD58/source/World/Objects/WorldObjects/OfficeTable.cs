namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Office Table.gmdl", "objects/Kitchen.mat")]
    class OfficeTable
        : WorldObject
    {
        public OfficeTable() : base(2, 3) { }
    }
}
