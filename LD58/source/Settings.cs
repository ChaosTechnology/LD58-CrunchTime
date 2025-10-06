using ChaosFramework.Math.Vectors;
using ChaosUtil.Serialization.Text;

namespace LD58
{
    public class Settings
    {
        public const string FILE = "./Settings.ini";

        [Ini.Field("General", "The maximum framerate.")]
        public int maxFPS = 120;

        [Ini.Field("Graphics", "The solid world resolution.")]
        public Vector2i deferredShaderResolution = new Vector2i(1920, 1080);

        [Ini.Field("Graphics", "The scale of the transparent world resolution relative to solid world resolution.")]
        public float transparencyScale = 1;

        [Ini.Field("Graphics", "The number of correctly sorted transparency layers.")]
        public int transparencyLayers = 20;
    }
}
