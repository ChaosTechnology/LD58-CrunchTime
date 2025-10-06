namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Long Counter.gmdl", "objects/Kitchen.mat")]
    class LongCounter
        : WorldObject
    {
        public LongCounter() : base(4, 1) { }
    }
}
