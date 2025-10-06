using ChaosFramework.Math.Vectors;

namespace LD58.World.Objectives
{
    using Constants;
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class Hygiene
        : Objective
    {
        DoorFrame bathroomDoor;

        protected override string GetText()
            => "Do morning hygiene.";

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            if (bathroomDoor != null)
            {
                DoorFrame door = interactible as DoorFrame;
                if (door != null)
                {
                    if (!door.OnDoorMat(interactAt))
                        if (interactor.parent.inventory.Contains(KnownItems.HELD_IN_POOP))
                            interactor.AddInteraction(
                                new DialogLine(interactor, "I really need to dump some weight...")
                                );
                        else if (interactor.parent.inventory.Contains(KnownItems.BODY_GREASE)
                              && interactor.parent.inventory.Contains(KnownItems.DIRTY_HANDS))
                            interactor.AddInteraction(
                                new DialogLine(interactor, "I'm covered in shit stains.\nI think, I'll wash up first.")
                                );
                        else if (interactor.parent.inventory.Contains(KnownItems.BODY_GREASE)
                              || interactor.parent.inventory.Contains(KnownItems.DIRTY_HANDS))
                            interactor.AddInteraction(
                                new Choice(interactor, "I still feel kinda filthy, but...",
                                    new Choice.Option("Good enough.", new CustomAction(interactor, Complete)),
                                    new Choice.Option("Clean up.")
                                    )
                                );
                        else
                            interactor.AddInteraction(
                                new DialogLine(interactor, "I haven't been this clean in a while!"),
                                new CustomAction(interactor, Complete)
                                );

                    return true;
                }
            }

            return base.Interact(interactor, interactible, interactAt);
        }

        void Complete()
        {
            bathroomDoor.Unlock();
            scene.SetObjective<GettingDressed>();
        }

        public override void Step(Interactor interactor, WorldObject steppedOn)
        {
            base.Step(interactor, steppedOn);
            if (bathroomDoor == null && steppedOn.GetName() == "Bathroom")
            {
                DoorFrame door = steppedOn as DoorFrame;
                if (door != null && door.OnDoorMat(interactor.parent.position))
                {
                    bathroomDoor = door;
                    door.Lock();
                }
            }
        }
    }
}
