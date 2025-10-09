using ChaosFramework.Components;

namespace LD58.World.Objectives
{
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
    }
}
