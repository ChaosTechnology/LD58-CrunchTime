using ChaosFramework.IO.Streams;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Shapes.Rigging;
using ChaosUtil.Reflection;
using ChaosFramework.Components;
using System.IO;
using SysCol = System.Collections.Generic;
using static ChaosFramework.Math.Trigonometry;

namespace LD58.World
{
    using System.Linq;
    using Objects;

    public class Stage
        : WorldScene
    {
        static readonly SysCol.Dictionary<string, System.Type> boneTypeCache
            = AssemblyManager.SubTypesOf<WorldObject>().Where(t => !t.IsAbstract).ToDictionary(t => t.Name, t => t);

        static string GetBoneTypeName(Rig.Bone bone)
        {
            int dotIndex = bone.name.IndexOf('.');
            return dotIndex > 0 ? bone.name.Substring(0, dotIndex) : bone.name;
        }

        static System.Type GetBoneType(Rig.Bone b)
            => boneTypeCache[GetBoneTypeName(b)];

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
                                WorldObject obj = (WorldObject)AddComponent(boneType, CreateParameters.Create(b));
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
            tiles = new WorldObject[size.x + 1, size.y + 1];
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
                                AddComponent(boneType, CreateParameters.Create(b));
                        }
                }
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            updateLayers[(int)UpdateLayers.UpdateCamera].Add(DebugSpinCamera);
        }

        void DebugSpinCamera()
        {
            Vector3f target = new Vector3f(13, 0, 8);
            float sin = Sin(ftime.totalTime) * 15;
            float cos = Cos(ftime.totalTime) * 15;
            Vector3f pos = new Vector3f(target.x + cos, 20, target.z + sin);
            view.Update(pos, target - pos, new Vector3f(0, 1, 0));
        }
    }
}
