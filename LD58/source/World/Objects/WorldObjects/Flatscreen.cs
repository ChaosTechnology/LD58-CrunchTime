namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Flatscreen.gmdl", "objects/Screen.mat")]
    class Flatscreen
        : WorldObject
    {
        public Flatscreen() : base(2, 1) { }
    }
}
