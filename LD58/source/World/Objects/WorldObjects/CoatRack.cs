using System;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Coat Rack.gmdl", "objects/Kitchen.mat")]
    class CoatRack
        : Interactible
    {
        public override bool Interact(Player player)
        {
            bool facing = Vector2f.Dot(player.direction, bone.GetDirection().xz) < 0;
            if (facing)
                scene.game.SwitchStage("office");

            return facing;
        }
    }
}
