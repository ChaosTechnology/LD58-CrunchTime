using System.Collections.Generic;
using ChaosFramework.Math.Vectors;
using System.Linq;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Door Frame.gmdl", "objects/Kitchen.mat")]
    class DoorFrame
        : WorldObject
    {
        static readonly Vector2i[] DOOR_MAT_POSITIONS = new[] { new Vector2i(0, -1), new Vector2i(1, -1) };
        static readonly Vector2i[] FRAME_POSITIONS = new[] { new Vector2i(0, 0), new Vector2i(1, 0) };

        bool locked;

        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
            => DOOR_MAT_POSITIONS.Concat(FRAME_POSITIONS);

        public override bool CanStepOn(Vector2i pos)
        {
            bool doorMatTile = TransformRelativeTilePositions(DOOR_MAT_POSITIONS).Any(x => x == pos);
            if (doorMatTile && name == "Bathroom")
                locked = true;

            return !locked || doorMatTile;
        }
    }
}
