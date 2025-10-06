using System;
using ChaosFramework.Math.Vectors;
using LD58.World.Player;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;

    [DefaultInstancer(64, "objects/Office Table.gmdl", "objects/Kitchen.mat")]
    class OfficeTable
        : Interactible
    {
        public OfficeTable() : base(2, 3) { }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            if (FacingAnyScreen(interactor.parent.direction))
            {
                interactor.AddInteraction(
                    new DialogLine(interactor, "LUDUM DARE CRUNCHTIME! LET'S GOOOOOOO!!!!")
                    );

                return true;
            }
            else
                return false;
        }

        public bool FacingAnyScreen(Vector2i facing)
            => Math.Abs(Vector2f.Dot(facing, bone.GetDirection().xz)) < 0.5f;

        public bool FacingScreenZero(Vector2i facing)
        {
            float dot = Vector2f.Dot(new Vector2i(-facing.y, facing.x), bone.GetDirection().xz);
            return dot > 0.5f;
        }

        public bool FacingScreenOne(Vector2i facing)
        {
            float dot = Vector2f.Dot(new Vector2i(-facing.y, facing.x), bone.GetDirection().xz);
            return dot < -0.5f;
        }
    }
}
