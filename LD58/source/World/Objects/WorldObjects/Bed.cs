namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Bed.gmdl", "objects/Bed.mat")]
    class Bed
        : WorldObject
    {
        public Bed() : base(2, 3) { }
    }
}
