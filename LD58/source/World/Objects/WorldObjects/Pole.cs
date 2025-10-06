namespace LD58.World.Objects.WorldObjects
{
    using Interaction;
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Pole.gmdl", "objects/Bathroom.mat")]
    class Pole
        : Interactible
    {
        public override bool Interact(Interactor interactor)
        {
            interactor.AddInteraction(
                new DialogLine(interactor, "Oh dear lord!"),
                new DialogLine(interactor, "I wish I was as tall as this pole!"),
                new Choice(interactor, "Cry in shame?",
                    new System.Tuple<string, InteractionStep[]>("Yes.", null)
                    )
                );
            return true;
        }
    }
}
