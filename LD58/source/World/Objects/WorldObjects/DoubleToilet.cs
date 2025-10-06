using ChaosFramework.Collections;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;
using static ChaosFramework.Math.Clamping;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class DoubleToilet
        : Toilet
    {
        static string[] lines = new[]
        {
            "Uhm... this seems to be a design flaw.",
            "They really didn't think this through...",
            "The architects who built this must be as competent as my team...",
            "Too bad that none of this has any meaning yet.",
            "...",
        };

        class UserInteractionState
        {
            public int numTimesInteracted = 0;
        }

        SysCol.Dictionary<Interactor, UserInteractionState> interactors = new SysCol.Dictionary<Interactor, UserInteractionState>();

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            UserInteractionState state = interactors.GetOrCreateValue(interactor);
            interactor.AddInteraction(new DialogLine(interactor, lines[state.numTimesInteracted]));

            state.numTimesInteracted = Min(lines.Length - 1, state.numTimesInteracted + 1);
            return true;
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            instancers[0].informer.AddInstance(Matrix.Translation(-0.5f, 0, 0) * bone.GetBoneTransform());
            instancers[0].informer.AddInstance(Matrix.Translation(0.5f, 0, 0) * bone.GetBoneTransform());
        }
    }
}
