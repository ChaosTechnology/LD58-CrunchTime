namespace LD58.World.Objects.WorldObjects
{
    [DefaultInstancer(64, "objects/Potted Plant.gmdl", "objects/Office.mat")]
    public class PottedPlant
        : WorldObject
    {
        [BoneParameter]
        string keyFor;
    }
}
