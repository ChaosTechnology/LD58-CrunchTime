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

        public virtual void Activate() { }

        public abstract bool interactionDone { get; }

        public virtual void SetUpdateCalls() { }

        public virtual void SetDrawCalls() { }
    }
}
