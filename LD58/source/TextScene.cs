using ChaosFramework.Graphics;
using ChaosFramework.Components;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;
using ChaosFramework.Graphics.OpenGl;
using static ChaosFramework.Math.Constants;

namespace LD58
{
    public class TextScene : Scene<Game>
    {
        public readonly Camera fullScreenView;

        public TextScene(Game game)
            : base(game, typeof(UpdateLayers), typeof(DrawLayers))
        {
            fullScreenView = new Camera();
            fullScreenView.Update(
                new Vector3f(0, 0, -1),
                new Vector3f(0, 0, 1),
                new Vector3f(0, 1, 0),
                0.5f,
                1.5f,
                PI_QUART,
                screenRatio: game.graphics.ratio
                );
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            drawLayers[(int)DrawLayers.ResetInstancers].Add(game.textBuffer.Clear);
            drawLayers[(int)DrawLayers.Text].Add(DrawText);
        }

        void DrawText()
        {
            fullScreenView.SetValues(game.graphics.shaders.managedText, Matrix.IDENTITY);
            game.textBuffer.Flush();
            game.textRenderer.DrawTexts(game.graphics.shaders.managedText, "HUD", game.textBuffer);
        }
    }
}
