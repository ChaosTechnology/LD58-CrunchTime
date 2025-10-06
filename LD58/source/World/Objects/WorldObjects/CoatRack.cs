using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    using Objectives;
    using Player;

    [DefaultInstancer(64, "objects/Coat Rack.gmdl", "objects/Kitchen.mat")]
    class CoatRack
        : Interactible
    {
        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            bool facing = Vector2f.Dot(interactor.parent.direction, bone.GetDirection().xz) < 0;
            if (facing)
            {
                Stage stage = new Stage(scene.game, scene.game.assetSource, "office");
                stage.doUpdate = false;
                stage.doDraw = false;
                stage.SetObjective<FindWorkspace>();
                scene.game.scenes.Add(stage);
            }

            return facing;
        }
    }
}
