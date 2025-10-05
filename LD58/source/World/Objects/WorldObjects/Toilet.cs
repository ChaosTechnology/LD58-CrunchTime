namespace LD58.World.Objects.WorldObjects
{
    using Interaction;
    using Interaction.Steps;

    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class Toilet
        : Interactible
    {
        public override bool Interact(Interactor interactor)
        {
            interactor.AddInteraction(
                new DialogLine(interactor, "Hello, World!"),
                new DialogLine(interactor, "World? What World?"),
                new DialogLine(interactor, "Oh, this world!"),
                new DialogLine(interactor, "Well, this world sucks..."),
                new DialogLine(interactor, "Goodbye, cruel world.")
                );
            return true;
        }
    }
}
