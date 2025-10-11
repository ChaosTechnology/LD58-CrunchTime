using ChaosFramework.Components;
using ChaosFramework.Graphics.Text;
using ChaosFramework.Math.Vectors;

namespace LD58.World
{
    using Objects;
    using Player;

    public abstract class Objective
        : Component<Stage>
    {
        TextBox text;

        protected override void Create(CreateParameters cparams)
            => text = AddComponent<TextBox>();

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            scene.updateLayers[(int)UpdateLayers.UpdateCamera].Add(UpdateText);
        }

        void UpdateText()
            => text.Update(GetText(), LayoutInfo.TOP_LEFT, new Vector2f(0, 0.95f), new Vector2f(0, 1), 0.05f);

        protected abstract string GetText();

        public virtual bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
            => interactible.Interact(interactor, interactAt);

        public virtual void Step(Interactor interactor, WorldObject steppedOn)
        { }

        protected void ChoosePlayerName(string name)
        {
            foreach (Player.Player obj in scene.EnumerateChildren<Player.Player>(false))
                if (obj.GetName() != name)
                    obj.Dispose();
        }
    }
}
