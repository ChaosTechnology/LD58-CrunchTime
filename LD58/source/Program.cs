using ChaosFramework.Input;
using System;
using System.Reflection;
using ChaosFramework.Platform.Glfw;

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

            GlfwPlatformContext platformContext = new GlfwPlatformContext();
            GlfwPlatformContext.GlfwWindow window = platformContext.CreateWindow();
            Func<InputContext, InputDeviceHost> createHost = _ => new ChaosFramework.Input.OpenTk.DeviceHost(_, window.window);

            Game g = new Game(platformContext, window, createHost);
            g.Run();
        }
    }
}
