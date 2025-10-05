using ChaosFramework.Components;
using SysCol = System.Collections.Generic;

namespace LD58.World.Player
{
    using Interaction;

    public class Interactor
        : StrictComponent<Player>
    {
        SysCol.Queue<InteractionStep> queued = new SysCol.Queue<InteractionStep>();
        InteractionStep current = null;

        public bool busy => current != null || queued.Count > 0;

        protected override void Create(CreateParameters cparams)
        {
        }

        public void AddInteraction(params InteractionStep[] steps)
            => AddInteraction((SysCol.IEnumerable<InteractionStep>)steps);

        public void AddInteraction(SysCol.IEnumerable<InteractionStep> steps)
        {
            if (steps != null)
                foreach (InteractionStep step in steps)
                    if (step != null)
                        if (step.interactor == this)
                            queued.Enqueue(step);
                        else
                            throw new System.InvalidOperationException();
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            UpdateInteraction();
            current?.SetUpdateCalls();
        }

        public override void SetDrawCalls()
        {
            base.SetDrawCalls();
            current?.SetDrawCalls();
        }

        void UpdateInteraction()
        {
            if (current != null && current.interactionDone)
            {
                current.Dispose();
                current = null;
            }

            if (current == null && queued.Count > 0)
                current = queued.Dequeue();
        }
    }
}
