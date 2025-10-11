using ChaosFramework.Components;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Graphics;

namespace LD58.EndScreen
{
    internal class CollectedCharacterTraits : Component<EndScreen>
    {
        TextBox text;

        protected override void Create(CreateParameters cparams)
        {
            text = AddComponent<TextBox>();
            text.Update("YOU ARE DONE WITH YOUR DAY!!! AMAZING!!!", new LayoutInfo(Align.TopLeft), 0, 0, 0.05f, 50);
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
        }

        void DrawShits()
        {

        }
    }
}
