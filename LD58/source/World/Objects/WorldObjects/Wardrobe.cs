namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Wardrobe.gmdl", "objects/Kitchen.mat")]
    class Wardrobe
        : WorldObject
    {
        public Wardrobe() : base(2, 1) { }
    }
}
