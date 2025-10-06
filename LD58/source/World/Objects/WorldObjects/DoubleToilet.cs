namespace LD58.World.Objects.WorldObjects
{
    using ChaosFramework.Graphics.OpenGl.Instancing;
    using ChaosFramework.Math;
    using Interaction;
    using Interaction.Steps;

    [DefaultInstancer(64, "objects/Toilet.gmdl", "objects/Bathroom.mat")]
    class DoubleToilet
        : Toilet
    {
        public override bool Interact(Interactor interactor)
        {
            interactor.AddInteraction(
                new DialogLine(interactor, "What the fuck!"),
                new Choice(interactor, "Try it out?",
                    new System.Tuple<string, InteractionStep>("How???", null),
                    new System.Tuple<string, InteractionStep>("...", new Suicide(interactor))
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
