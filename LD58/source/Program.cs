using System;
using System.Reflection;
using System.Windows.Forms;

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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Game game = new Game();
            game.window.Icon = Properties.Resources.Icon;
            game.window.Text = "LD58";
            game.window.MinimumSize = new System.Drawing.Size(800, 450);
            game.window.Size = game.settings.deferredShaderResolution;
            game.window.BackgroundImageLayout = ImageLayout.Stretch;
            game.window.BackgroundImage = Properties.Resources.wallpaper;
            Application.Run(game.window);
        }
    }
}
