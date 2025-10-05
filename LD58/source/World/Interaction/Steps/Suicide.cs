using System.Diagnostics;

namespace LD58.World.Interaction.Steps
{
    class Suicide
        : InteractionStep
    {
        public Suicide(Interactor interactor)
            : base(interactor)
        { }

        public override bool interactionDone => false;

        public override void SetDrawCalls() { }

        public override void SetUpdateCalls()
            => Process.GetCurrentProcess().Kill();
    }
}
