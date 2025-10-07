using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace LD58.World.Objects.WorldObjects
{
    using ChaosFramework.Graphics.OpenGl.Instancing;
    using Player;

    [DefaultInstancer(64, "objects/Door Frame.gmdl", "objects/Kitchen.mat")]
    class DoorFrame
        : Interactible
    {
        protected virtual Vector2i[] doorMatPositions => new[] { new Vector2i(0, -1), new Vector2i(1, -1) };
        protected virtual Vector2i[] doorFramePositions => new[] { new Vector2i(0, 0), new Vector2i(1, 0) };

        [BoneParameter]
        bool locked = false;

        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
            => doorMatPositions.Concat(doorFramePositions);

        public override bool CanStepOn(Vector2i pos)
        {
            bool doorMatTile = OnDoorMat(pos);
            return !locked || doorMatTile;
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            if (locked)
                for (float f = 0.8f; f > 0; f -= 0.2f)
                    instancers[0].informer.AddInstance(Matrix.Scaling(f) * bone.GetBoneTransform());
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
            => false;

        public bool OnDoorMat(Vector2i pos)
            => TransformRelativeTilePositions(doorMatPositions).Contains(pos);

        public void Lock()
            => locked = true;

        public void Unlock()
            => locked = false;
    }
}
