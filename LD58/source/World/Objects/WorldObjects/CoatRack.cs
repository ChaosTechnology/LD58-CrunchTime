using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    using Player;

    [DefaultInstancer(64, "objects/Coat Rack.gmdl", "objects/Kitchen.mat")]
    class CoatRack
        : Interactible
    {
        public override bool Interact(Interactor interactor)
        {
            bool facing = Vector2f.Dot(interactor.parent.direction, bone.GetDirection().xz) < 0;
            if (facing)
                scene.game.SwitchStage("office");

            return facing;
        }
    }
}
