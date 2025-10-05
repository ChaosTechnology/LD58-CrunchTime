namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Kitchen.gmdl", "objects/Kitchen.mat")]
    class Kitchen
        : WorldObject
    {
        public Kitchen() : base(2, 1) { }
    }
}
