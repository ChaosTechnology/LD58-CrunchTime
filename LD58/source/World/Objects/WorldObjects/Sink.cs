using ChaosFramework.Collections;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction;
    using LD58.World.Constants;
    using LD58.World.Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Sink.gmdl", "objects/Bathroom.mat")]
    class Sink
        : Interactible
    {
        public override bool Interact(Interactor interactor)
        {
            LinkedList<InteractionStep> steps = new LinkedList<InteractionStep>();

            PlayerInventory inventory = interactor.parent.inventory;

            if (inventory.Contains(KnownItems.DIRTY_HANDS))
                steps.Add(
                    new Choice(interactor, "I can clean my hands here...",
                        new Choice.Option(
                            "Wash hands",
                            new CustomAction(interactor, () => inventory.Remove(KnownItems.DIRTY_HANDS, all: true))
                            ),
                        new Choice.Option("Leave")
                        )
                    );

            if (steps.empty)
                steps.Add(new DialogLine(interactor, "My hands are clean."));

            interactor.AddInteraction(steps);

            return true;
        }
    }
}
