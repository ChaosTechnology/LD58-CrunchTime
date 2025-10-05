namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class Toilet
        : Interactible
    {
        public override bool Interact(Player player)
        {
            System.Console.Beep();
            return true;
        }
    }
}
