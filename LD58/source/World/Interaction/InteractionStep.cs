using ChaosFramework.Core;

namespace LD58.World.Interaction
{
    using Player;

    public abstract class InteractionStep
        : Disposable
    {
        public readonly Interactor interactor;

        public InteractionStep(Interactor interactor)
        {
            this.interactor = interactor;
        }

        public abstract bool interactionDone { get; }

        public abstract void SetUpdateCalls();

        public abstract void SetDrawCalls();
    }
}
