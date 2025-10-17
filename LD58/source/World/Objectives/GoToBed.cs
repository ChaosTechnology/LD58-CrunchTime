using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using LD58.World.Objects;
using LD58.World.Objects.WorldObjects;
using LD58.World.Player;

namespace LD58.World.Objectives
{
    using EndScreen = EndScreen.EndScreen;

    internal class GoToBed
        : Objective
    {
        protected override string GetText()
            => "Go to bed.";

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            ChoosePlayerName("Apartment Door");
        }

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            if (interactible is Bed)
            {
                scene.game.scenes.Add(new EndScreen(scene.game, interactor.parent.inventory.CopyBag()));
                scene.doUpdate = false;
                return true;
            }

            return base.Interact(interactor, interactible, interactAt);
        }
    }
}
