using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Math;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics;
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
        static readonly Vector2i[] DOOR_MAT_POSITIONS = new[] { new Vector2i(0, -1), new Vector2i(1, -1) };
        static readonly Vector2i[] FRAME_POSITIONS = new[] { new Vector2i(0, 0), new Vector2i(1, 0) };

        bool locked = false;

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
        }

        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
            => DOOR_MAT_POSITIONS.Concat(FRAME_POSITIONS);

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
            => TransformRelativeTilePositions(DOOR_MAT_POSITIONS).Contains(pos);

        public void Lock()
            => locked = true;

        public void Unlock()
            => locked = false;
    }
}
