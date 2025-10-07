using ChaosFramework.Collections;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math.Vectors;
using LD58.World.Constants;
using LD58.World.Interaction;
using LD58.World.Interaction.Steps;
using LD58.World.Player;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Shower.gmdl", "objects/Bathroom.mat")]
    [GlassInstancer(64, "objects/Shower Transparent.gmdl")]
    class Shower
        : Interactible
    {
        class UserInteractionState
        {
            public int numTimesInteractedWithHeldInPoop;
        }

        SysCol.Dictionary<Interactor, UserInteractionState> users = new SysCol.Dictionary<Interactor, UserInteractionState>();

        public Shower() : base(2, 2) { }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            instancers[1].informer.AddInstance(bone.GetBoneTransform());
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            LinkedList<InteractionStep> steps = new LinkedList<InteractionStep>();

            PlayerInventory inventory = interactor.parent.inventory;
            UserInteractionState userInteractionState = users.GetOrCreateValue(interactor);

            string choiceText = null;

            LinkedList<Choice.Option> options = new LinkedList<Choice.Option>();

            if (inventory.Contains(KnownItems.DIRTY_HANDS))
            {
                choiceText = "My hands are dirty...";
                options.Add(new Choice.Option(
                    "Wash hands",
                    new CustomAction(interactor, (Interactor _) => inventory.Remove(KnownItems.DIRTY_HANDS, all: true))
                    ));
            }

            if (inventory.Contains(KnownItems.BODY_GREASE))
            {
                choiceText = "I feel greasy and dirty all over!";
                options.Add(new Choice.Option(
                    "Take a shower",
                    new CustomAction(interactor, (Interactor _) =>
                    {
                        inventory.Remove(KnownItems.DIRTY_HANDS, all: true);
                        inventory.Remove(KnownItems.BODY_GREASE, all: true);
                    })
                    ));
            }

            options.Add(new Choice.Option("Leave"));

            steps.Add(options.length == 1
                ? new DialogLine(interactor, "I'm squeaky clean!")
                : new Choice(interactor, choiceText, options.ToArray())
                );

            interactor.AddInteraction(steps);

            return true;
        }
    }
}
