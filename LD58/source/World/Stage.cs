using ChaosFramework.IO.Streams;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using ChaosUtil.Reflection;
using System.IO;
using SysCol = System.Collections.Generic;

namespace LD58.World
{
    using Objects;

    public class Stage
        : WorldScene
    {
        static readonly SysCol.Dictionary<string, System.Type> boneTypeCache = new SysCol.Dictionary<string, System.Type>();

        static string GetBoneTypeName(Rig.Bone bone)
            => bone.name;

        static System.Type GetBoneType(Rig.Bone b)
        {
            System.Type boneType = null;
            string boneTypeName = GetBoneTypeName(b);
            if (!boneTypeCache.TryGetValue(boneTypeName, out boneType))
                if (AssemblyManager.TryGetTypeByFullName(boneTypeName, out boneType))
                    if (boneType.IsSubclassOf(typeof(WorldObject)))
                        boneTypeCache[boneTypeName] = boneType;

            return boneType;
        }

        public readonly Vector2i size;
        readonly WorldObject[,] tiles;

        public Stage(Game game, StreamSource source, string name)
            : base(game)
        {
            Vector2i size = 0;
            SysCol.Dictionary<Vector2i, WorldObject> occupied = new SysCol.Dictionary<Vector2i, WorldObject>();

            foreach (string rigPath in source.EnumerateKeys($"stages/{name}/objects/*.rig"))
                using (Stream rigStream = source.OpenRead(rigPath))
                {
                    Rig rig = Rig.FromStream(rigStream);
                    foreach (Rig.Bone b in rig.root.EnumerateBones())
                        if (b != rig.root)
                        {
                            System.Type boneType = GetBoneType(b);
                            if (boneType == null)
                                throw new System.Exception($"Unknown bone type '{GetBoneTypeName(b)}'!");
                            else
                            {
                                WorldObject obj = (WorldObject)AddComponent(boneType);
                                foreach (Vector2i pos in obj.OccupiedTiles())
                                    if (occupied.ContainsKey(pos))
                                        throw new System.Exception($"Tile {{{pos.x}, {pos.y}}} is already occupied!");
                                    else
                                    {
                                        occupied[pos] = obj;
                                        size = size.Max(pos);
                                    }
                            }
                        }
                }

            this.size = size;
            tiles = new WorldObject[size.x, size.y];
            foreach (SysCol.KeyValuePair<Vector2i, WorldObject> pos in occupied)
                tiles[pos.Key.x, pos.Key.y] = pos.Value;


            foreach (string rigPath in source.EnumerateKeys($"stages/{name}/deco/*.rig"))
                using (Stream rigStream = source.OpenRead(rigPath))
                {
                    Rig rig = Rig.FromStream(rigStream);
                    foreach (Rig.Bone b in rig.root.EnumerateBones())
                        if (b != rig.root)
                        {
                            System.Type boneType = GetBoneType(b);
                            if (boneType == null)
                                throw new System.Exception($"Unknown bone type '{GetBoneTypeName(b)}'!");
                            else
                            {
                                WorldObject obj = (WorldObject)AddComponent(boneType);
                                foreach (Vector2i pos in obj.OccupiedTiles())
                                    if (occupied.ContainsKey(pos))
                                        throw new System.Exception($"Tile {{{pos.x}, {pos.y}}} is already occupied!");
                                    else
                                    {
                                        occupied[pos] = obj;
                                        size = size.Max(pos);
                                    }
                            }
                        }
                }
        }
    }
}
