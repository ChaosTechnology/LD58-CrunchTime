using ChaosFramework.Components;
using ChaosFramework.Core;
using ChaosFramework.Graphics.OpenGl;
using ChaosFramework.Graphics.OpenGl.AssetContainers;
using ChaosFramework.Input;
using ChaosFramework.Input.RawInput;
using ChaosFramework.IO.Streams;
using ChaosFramework.Sound;
using ChaosFramework.Sound.OpenAL;
using ChaosUtil.Platform.Windows.WinAPI.winuser;
using ChaosUtil.Serialization.Text;
using OpenTK.Graphics.OpenGL;

namespace LD58
{
    using World;

    public class Game : BaseGame
    {
        const float UPDATE_INPUT_DEVICE_INTERVAL = 3;

        public readonly Settings settings;

        public StreamSource assetSource { get; private set; }

        public Graphics graphics { get; private set; }
        public Audio audio { get; private set; }
        public InputContext input { get; private set; }
        public SoundPool sounds { get; private set; }

        public AnimationContainer animations { get; private set; }
        public MaterialContainer materials { get; private set; }
        public TextureContainer textures { get; private set; }
        public ShaderContainer shaders { get; private set; }
        public ShaderCodeContainer shaderCode { get; private set; }
        public SoundDataContainer samples { get; private set; }
        public MeshContainer meshes { get; private set; }
        public FontContainer fonts { get; private set; }

        float updateInputDeviceTimer;

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
            input = new InputContext(typeof(InputLayers), context => new RawInputDeviceHost(context));

            graphics = new Graphics(panel, 3, 3);
            fonts = new FontContainer(assetSource, graphics, false);
            textures = new TextureContainer(assetSource, graphics.dispatcher, false);
            materials = new MaterialContainer(assetSource, graphics, textures, false);
            meshes = new MeshContainer(assetSource, graphics.dispatcher, false);
            shaderCode = new ShaderCodeContainer(new StreamSourceCollection(StreamSources.shaderCode, assetSource));
            shaders = new ShaderContainer(assetSource, graphics, shaderCode);
            animations = new AnimationContainer(assetSource, false);

            sounds = new SoundPool(audio, samples);

            scenes.Add(new Stage(this, assetSource, "office"));

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

            Dispatcher.dispatcher.ExecuteDispatchers(10);
            sounds.Update();
            updateInputDeviceTimer -= time.time.frameTime;
            if (updateInputDeviceTimer < 0)
            {
                input.UpdateDeviceList();
                updateInputDeviceTimer = UPDATE_INPUT_DEVICE_INTERVAL;
            }

            input.UpdateInputConsumption();
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
            input?.Dispose();
            textures?.Dispose();
            materials?.Dispose();
            meshes?.Dispose();
            fonts?.Dispose();
            shaders?.Dispose();
            shaderCode?.Dispose();
            animations?.Dispose();
            graphics?.Dispose();
            sounds?.Dispose();
            samples.Dispose();
            audio.Dispose();
        }
    }
}
