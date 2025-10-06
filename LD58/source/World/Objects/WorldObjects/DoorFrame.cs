using ChaosFramework.Math.Vectors;
using LD58.World.Player;
using System.Collections.Generic;
using System.Linq;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Door Frame.gmdl", "objects/Kitchen.mat")]
    class DoorFrame
        : Interactible
    {
        static readonly Vector2i[] DOOR_MAT_POSITIONS = new[] { new Vector2i(0, -1), new Vector2i(1, -1) };
        static readonly Vector2i[] FRAME_POSITIONS = new[] { new Vector2i(0, 0), new Vector2i(1, 0) };

        bool locked;

        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
            => DOOR_MAT_POSITIONS.Concat(FRAME_POSITIONS);

        public override bool CanStepOn(Vector2i pos)
        {
            bool doorMatTile = OnDoorMat(pos);
            return !locked || doorMatTile;
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => false;

        public bool OnDoorMat(Vector2i pos)
            => TransformRelativeTilePositions(DOOR_MAT_POSITIONS).Any(x => x == pos);

        public void Lock()
            => locked = true;

        public void Unlock()
            => locked = false;
    }
}
