using ChaosFramework.Input;
using System;
using System.Reflection;

#if OS_WINDOWS
using System.Windows.Forms;
#endif

namespace LD58
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            typeof(System.Globalization.CultureInfo).GetField(
                "s_userDefaultCulture",
                BindingFlags.NonPublic | BindingFlags.Static
                ).SetValue(null, System.Globalization.CultureInfo.InvariantCulture);

            Environment.CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            ChaosUtil.Reflection.AssemblyManager.RegisterAssemblies(
                typeof(Program).Assembly,
                typeof(ChaosFramework.Math.Vectors.Vector2f).Assembly,
                typeof(ChaosFramework.Graphics.OpenGl.GlStateTracker.RenderStateChange).Assembly
                );

            ChaosFramework.IO.ChaosIO.Init(
                typeof(Program).Assembly,
                typeof(ChaosFramework.Math.Matrix).Assembly,
                typeof(ChaosFramework.Graphics.OpenGl.Graphics).Assembly,
                typeof(ChaosFramework.Graphics.Text.GlyphDimensions).Assembly
                );

#if OS_WINDOWS
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game game = new Game();
            game.window.Icon = Properties.Resources.Icon;
            game.window.Text = "LD58";
            game.window.MinimumSize = new System.Drawing.Size(800, 450);
            game.window.Size = game.settings.deferredShaderResolution;
            game.window.BackgroundImageLayout = ImageLayout.Stretch;
            game.window.BackgroundImage = Properties.Resources.wallpaper;
#else
            GlfwPlatformContext platformContext = new GlfwPlatformContext();
            GlfwPlatformContext.GlfwWindow window = platformContext.CreateWindow();
            Func<InputContext, InputDeviceHost> createHost = _ => new ChaosFramework.Input.OpenTk.DeviceHost(_, window.window);
            Game g = new Game(platformContext, window, createHost);
#endif

            g.Run();
        }
    }
}
