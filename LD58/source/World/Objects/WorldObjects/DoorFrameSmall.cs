using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Door Frame Small.gmdl", "objects/Kitchen.mat")]
    class DoorFrameSmall
        : DoorFrame
    {
        protected override Vector2i[] doorMatPositions => new[] { new Vector2i(0, -1) };
        protected override Vector2i[] doorFramePositions => new[] { new Vector2i(0, 0) };
    }
}
