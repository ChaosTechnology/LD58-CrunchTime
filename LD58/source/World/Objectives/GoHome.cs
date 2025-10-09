using System;
using ChaosFramework.Math.Vectors;
using LD58.World.Interaction.Steps;
using LD58.World.Objects;
using LD58.World.Objects.WorldObjects;
using LD58.World.Player;

namespace LD58.World.Objectives
{
    internal class GoHome
        : Objective
    {
        DoorFrame interactedDoorFrame;

        protected override string GetText()
            => "Go home.";

        public override void Step(Interactor interactor, WorldObject steppedOn)
        {
            base.Step(interactor, steppedOn);
            if (steppedOn.GetName() == "Exit")
            {
                DoorFrame door = steppedOn as DoorFrame;
                if (door != null && !door.OnDoorMat(interactor.parent.position))
                {
                    interactedDoorFrame = door;
                    interactor.AddInteraction(new Choice(interactor, "Leave for home?",
                        new Choice.Option("It's time to go", new CustomAction(interactor, LeaveForHome)),
                        new Choice.Option("I'll stay a bit longer", new CustomAction(interactor, MovePlayerToLastSteppedOnTile)
                        )
                    ));
                }
            }
        }

        void LeaveForHome(Interactor interactor)
            => scene.game.SwitchScene<Hygiene>(interactor, "home");

        void MovePlayerToLastSteppedOnTile(Interactor interactor)
        {
            Vector2f boneDir = -interactedDoorFrame.bone.GetDirection().xz;
            interactor.parent.direction = new Vector2i((int)(Math.Round(boneDir.x)), (int)(Math.Round(boneDir.y)));
            interactor.parent.position += interactor.parent.direction;
        }
    }
}
