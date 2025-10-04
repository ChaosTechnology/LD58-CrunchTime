using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.IO.Streams;
using ChaosFramework.Sound;
using ChaosFramework.Sound.OpenAL;
using ChaosUtil.Platform.Windows.WinAPI.winuser;
using ChaosUtil.Serialization.Text;
using OpenTK.Graphics.OpenGL;

namespace LD58
{
    public class Game : BaseGame
    {
        public readonly Settings settings;

        public StreamSource assetSource { get; private set; }

        public Graphics graphics { get; private set; }
        public Audio audio { get; private set; }

        public AnimationContainer animations { get; private set; }
        public MaterialContainer materials { get; private set; }
        public TextureContainer textures { get; private set; }
        public ShaderContainer shaders { get; private set; }
        public ShaderCodeContainer shaderCode { get; private set; }
        public SoundDataContainer samples { get; private set; }
        public MeshContainer meshes { get; private set; }
        public FontContainer fonts { get; private set; }

        public Game() : base()
        {
            System.Windows.Forms.Cursor.Hide();
            window.Cursor.Dispose();

            settings = new Settings();
            bool settingsExist = System.IO.File.Exists(Settings.FILE);
            if (settingsExist)
                Ini.Load(ref settings, Settings.FILE);
            if (!settingsExist)
                Ini.Save(ref settings, Settings.FILE);
        }

        public override void LoadGame()
        {
            base.LoadGame();
            gameLoop = new ChaosFramework.Components.GameLoop.CappedVariableTimeLoop(settings.maxFPS);

            assetSource = new ChaosFramework.IO.ChaosArchive(new System.IO.FileInfo("assets.cha"), false);

            audio = new Audio();
            samples = new SoundDataContainer(assetSource, false);

            graphics = new Graphics(panel, 3, 3);
            fonts = new FontContainer(assetSource, graphics, false);
            textures = new TextureContainer(assetSource, graphics.dispatcher, false);
            materials = new MaterialContainer(assetSource, graphics, textures, false);
            meshes = new MeshContainer(assetSource, graphics.dispatcher, false);
            shaderCode = new ShaderCodeContainer(new StreamSourceCollection(StreamSources.shaderCode, assetSource));
            shaders = new ShaderContainer(assetSource, graphics, shaderCode);
            animations = new AnimationContainer(assetSource, false);
            scenes.Add(new WorldScene(this));

            window.BackgroundImage.Dispose();
            window.BackgroundImage = null;
        }

        protected override void Update()
        {
            base.Update();

            if (GetActiveWindow.Invoke() == window.Handle)
                System.Windows.Forms.Cursor.Position
                    = new System.Drawing.Point(
                        window.Location.X + window.Width / 2,
                        window.Location.Y + window.Height / 2
                        );
        }

        protected override void Draw()
        {
            GL.ClearColor(0, 1, 0, 1);
            Graphics.ThrowErrors();
            GL.Clear(ClearBufferMask.ColorBufferBit);
            Graphics.ThrowErrors();
            base.Draw();
            graphics.graphicsContext.SwapBuffers();
        }

        protected override void DoDispose()
        {
            base.DoDispose();
            textures?.Dispose();
            materials?.Dispose();
            meshes?.Dispose();
            fonts?.Dispose();
            graphics?.Dispose();
            audio.Dispose();
            samples.Dispose();
            shaders?.Dispose();
            shaderCode?.Dispose();
            animations?.Dispose();
        }
    }
}
