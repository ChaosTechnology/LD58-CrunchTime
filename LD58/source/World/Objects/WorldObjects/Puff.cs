namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Puff.gmdl", "objects/Office.mat")]
    public class Puff
        : WorldObject
    {
        public Puff() : base(2, 2) { }
    }
}
