using ChaosFramework.Math.Vectors;
using ChaosUtil.Reflection;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    [AssemblyManager.ListSubTypes]
    public abstract class WorldObject
        : BaseObject
    {
        public readonly uint width, height;

        public WorldObject(uint width = 1, uint height = 1)
        {
            this.width = width;
            this.height = height;
        }

        public string GetName()
            => name;

        /// <summary>
        ///     Determine whether the player can occupy the same space
        ///     as this object at the provided location.
        /// </summary>
        /// <param name="pos"> The location the player attempts to enter. </param>
        /// <returns>
        ///     <see langword="true"/>, if the location can be entered;
        ///     <see langword="false"/> otherwise.
        /// </returns>
        public virtual bool CanStepOn(Vector2i pos)
            => false;

        /// <summary>
        ///     Enumerate all tiles that this object occupies.
        ///     The default is to occupy exactly one tile.
        /// </summary>
        /// <returns> The set of tiles this object occupies. </returns>
        public SysCol.IEnumerable<Vector2i> OccupiedTiles()
            => TransformRelativeTilePositions(RelativeOffsetsForOccupiedTiles());

        protected SysCol.IEnumerable<Vector2i> TransformRelativeTilePositions(params Vector2i[] relativeOffsets)
            => TransformRelativeTilePositions((SysCol.IEnumerable<Vector2i>)relativeOffsets);

        protected SysCol.IEnumerable<Vector2i> TransformRelativeTilePositions(SysCol.IEnumerable<Vector2i> relativeOffsets)
        {
            Vector2f posf = bone.GetPosition().xz;
            Vector2i pos = new Vector2i((int)System.Math.Round(posf.x), (int)System.Math.Round(posf.y));
            if (!pos.GreaterEquals(0))
                throw new System.Exception("Object bones must have unsigned positions!");

            Vector2f dirf = bone.GetDirection().xz;
            Vector2i dir = new Vector2i((int)System.Math.Round(dirf.y), (int)System.Math.Round(-dirf.x));

            foreach (Vector2i x in relativeOffsets)
            {
                Vector2i offset = x * 2 + 1;
                offset = new Vector2i(
                    offset.x * dir.x - offset.y * dir.y,
                    offset.x * dir.y + offset.y * dir.x
                    );
                yield return pos + (offset - 1) / 2;
            }
        }

        protected virtual SysCol.IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    yield return new Vector2i(x, y);
        }
    }
}
