using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;
    using Player;

    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class DoubleToilet
        : Toilet
    {
        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            interactor.AddInteraction(
                new DialogLine(interactor, "What the fuck!"),
                new Choice(interactor, "Try it out?",
                    new Choice.Option("How???", null),
                    new Choice.Option("...", new[] { new Suicide(interactor) })
                    )
                );
            return true;
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            instancers[0].informer.AddInstance(Matrix.Translation(-0.5f, 0, 0) * bone.GetBoneTransform());
            instancers[0].informer.AddInstance(Matrix.Translation(0.5f, 0, 0) * bone.GetBoneTransform());
        }
    }
}
