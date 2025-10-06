using System.Collections.Generic;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Sofa.gmdl", "objects/Sofa.mat")]
    public class Sofa
        : WorldObject
    {
        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
        {
            yield return new Vector2i(0, 0);
            yield return new Vector2i(0, 1);
            yield return new Vector2i(1, 0);
            yield return new Vector2i(1, 1);
            yield return new Vector2i(2, 0);
            yield return new Vector2i(2, 1);
            yield return new Vector2i(3, 0);
            yield return new Vector2i(3, 1);
            yield return new Vector2i(3, 2);
            yield return new Vector2i(4, 0);
            yield return new Vector2i(4, 1);
            yield return new Vector2i(4, 2);
            yield return new Vector2i(4, 3);
            yield return new Vector2i(5, 0);
            yield return new Vector2i(5, 1);
            yield return new Vector2i(5, 2);
            yield return new Vector2i(5, 3);
        }
    }
}
