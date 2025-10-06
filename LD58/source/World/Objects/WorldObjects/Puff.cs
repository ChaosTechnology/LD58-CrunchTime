namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;
    using World.Constants;
    using World.Player;

    [DefaultInstancer(64, "objects/Puff.gmdl", "objects/Office.mat")]
    public class Puff
        : Interactible
    {
        bool danced = false;

        public Puff()
            : base(2, 2)
        { }

        public override bool Interact(Interactor interactor)
        {
            if (danced)
                interactor.AddInteraction(
                    new DialogLine(interactor, "Enough dancing for today.")
                    );
            else
                interactor.AddInteraction(
                    new DialogLine(interactor, "My boss really is living the life here."),
                    new Choice(interactor, "Well, nobody's here, so...", new[] {
                        new Choice.Option("Leave it alone."),
                        new Choice.Option("Dance!",
                            new DialogLine(interactor, "Damn, that was good, I feel so sexy now!"),
                            new AddItem(interactor, KnownItems.BODY_GREASE),
                            new CustomAction(interactor, Dance)
                            )
                        })
                    );
            return true;
        }

        void Dance()
            => danced = true;
    }
}
