using ChaosFramework.Collections;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction;
    using Interaction.Steps;
    using LD58.World.Constants;
    using Player;

    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class Toilet
        : Interactible
    {
        class UserInteractionState
        {
            public int numTimesInteractedBeforeFlush;
            public bool inspected;
            public bool hasUsed;
        }

        bool hasContent;
        bool clean;

        SysCol.Dictionary<Interactor, UserInteractionState> users = new SysCol.Dictionary<Interactor, UserInteractionState>();

        public override bool Interact(Interactor interactor)
        {
            LinkedList<InteractionStep> steps = new LinkedList<InteractionStep>();

            PlayerInventory playerInventory = interactor.parent.inventory;
            UserInteractionState userInteractionState = users.GetOrCreateValue(interactor);

            bool canPoop = playerInventory.Contains(KnownItems.HELD_IN_POOP);

            // Dialog lines
            if (canPoop)
                steps.Add(new DialogLine(interactor, "I could really use this right now!"));

            if (hasContent)
            {
                userInteractionState.numTimesInteractedBeforeFlush++;
                steps.Add(new DialogLine(interactor,
                    userInteractionState.hasUsed
                    ? (userInteractionState.numTimesInteractedBeforeFlush > 5)
                        ? "I'm never gonna flush this!"
                        : "I havent flushed it yet."
                    : "Someone hasn't flushed. How disgusting."
                    ));
            }

            // Choices
            LinkedList<Choice.Option> options = new LinkedList<Choice.Option>();
            if (canPoop)
                options.Add(new Choice.Option(
                    "Use",
                    new CustomAction(interactor, () =>
                    {
                        playerInventory.Remove(KnownItems.HELD_IN_POOP);
                        userInteractionState.hasUsed = true;
                        hasContent = true;
                        clean = false;
                    }),
                    new DialogLine(interactor, "Ahhh... what a relieve.")
                    ));

            if (hasContent)
            {
                options.Add(new Choice.Option(
                        "Flush",
                        new CustomAction(interactor, () =>
                        {
                            hasContent = false;
                            userInteractionState.numTimesInteractedBeforeFlush = 0;
                        }),
                        new DialogLine(interactor, "That's better.")
                        ));
                if (!userInteractionState.inspected)
                    options.Add(new Choice.Option(
                            "Inspect",
                            new CustomAction(interactor, () => userInteractionState.inspected = true),
                            new DialogLine(interactor, "It stinks quite bad.")
                            ));
                else
                    options.Add(new Choice.Option(
                            "Collect",
                            new CustomAction(interactor, () =>
                            {
                                playerInventory.AddItem(KnownItems.POOP);
                                hasContent = false;
                            }),
                            new DialogLine(interactor, "Can't let it go to waste...")
                            ));
            }
            else if (!clean)
                options.Add(new Choice.Option(
                    "Clean",
                    new CustomAction(interactor, () => clean = true),
                    new DialogLine(interactor, "What a chore.")
                    ));

                steps.Add(
                    options.empty
                    ? new DialogLine(interactor, "I don't need this right now.")
                    : new Choice(interactor, "What to do?", options.ToArray())
                    );


            interactor.AddInteraction(steps);

            return true;
        }
    }
}
