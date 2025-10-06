using ChaosFramework.Collections;
using ChaosFramework.Math.Vectors;
using static ChaosFramework.Math.Clamping;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objects.WorldObjects
{
    using System.Windows.Forms;
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Office Banner.gmdl", "objects/Office Banner.mat")]
    class OfficeBanner
        : Interactible
    {
        public OfficeBanner() : base(4, 1) { }

        static string[] lines = new[]
        {
            "It's always crunch time with this company.",
            "There's just never enough time...",
            "Nobody can ever get anything done properly here.",
            "...",
            "But that's fine, I still like it here.",
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
    }
}
