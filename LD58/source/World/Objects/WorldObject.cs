using ChaosFramework.Math.Vectors;
using ChaosUtil.Reflection;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects
{
    [AssemblyManager.ListSubTypes]
    public abstract class WorldObject
        : BaseObject
    {
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
        public virtual SysCol.IEnumerable<Vector2i> OccupiedTiles()
        {
            Vector2f posf = bone.GetPosition().xz;
            Vector2i pos = new Vector2i((int)System.Math.Round(posf.x), (int)System.Math.Round(posf.y));
            if (!pos.GreaterEquals(0))
                throw new System.Exception("Object bones must have unsigned positions!");

            Vector2f dirf = bone.GetDirection().xz;
            Vector2i dir = new Vector2i((int)System.Math.Round(dirf.x), (int)System.Math.Round(dirf.y));

            if (dir.x == 0 && dir.y == 1)
                yield return pos;
            else if (dir.x == 0 && dir.y == -1)
                yield return pos - 1;
            else if (dir.x == 1 && dir.y == 0)
                yield return pos - new Vector2i(0, 1);
            else if (dir.x == -1 && dir.y == 0)
                yield return pos - new Vector2i(1, 0);
            else
                throw new System.Exception("Carthesian directions only!");
        }
    }
}
