namespace LD58.World.Objects.WorldObjects
{
    using ChaosFramework.Math.Vectors;
    using Constants;
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Potted Plant.gmdl", "objects/Office.mat")]
    public class PottedPlant
        : Interactible
    {
        [BoneParameter]
        string keyFor;

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            interactor.AddInteraction(new DialogLine(interactor, "What an ugly plant."));
            if (keyFor != null)
            {
                interactor.AddInteraction(
                    new DialogLine(interactor, "Wait, there's something shiny in there!"),
                    new Choice(interactor, "Take it?",
                        new Choice.Option("yes",
                            new DialogLine(interactor, "It's a key!"),
                            new CustomAction(interactor, Take)
                            ),
                        new Choice.Option("no")
                        )
                    );
            }
            return true;
        }

        void Take(Interactor interactor)
        {
            interactor.parent.inventory.AddItem(KnownItems.DIRTY_HANDS);
            switch (keyFor)
            {
                case "Secret Room":
                    interactor.parent.inventory.AddItem(KnownItems.SECRET_ROOM_KEY);
                    break;
            }

            keyFor = null;
        }
    }
}
