using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Math.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace LD58.World.Objects.WorldObjects
{
    using Interaction.Steps;
    using Inventory;
    using Player;

    [DefaultInstancer(64, "objects/Door Frame.gmdl", "objects/Kitchen.mat")]
    [DefaultInstancer(64, "objects/Door.gmdl", "objects/Kitchen.mat")]
    class DoorFrame
        : Interactible
    {
        protected virtual Vector2i[] doorMatPositions => new[] { new Vector2i(0, -1), new Vector2i(1, -1) };
        protected virtual Vector2i[] doorFramePositions => new[] { new Vector2i(0, 0), new Vector2i(1, 0) };

        [BoneParameter]
        protected bool locked = false;

        protected override IEnumerable<Vector2i> RelativeOffsetsForOccupiedTiles()
            => doorMatPositions.Concat(doorFramePositions);

        public override bool CanStepOn(Vector2i pos)
        {
            bool doorMatTile = OnDoorMat(pos);
            return !locked || doorMatTile;
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
        {
            base.GiveMeInstances(instancers);
            if (locked)
                instancers[1].informer.AddInstance(bone.GetBoneTransform());
        }

        public override bool Interact(Interactor interactor, Vector2i interactAt)
        {
            if (locked && TransformRelativeTilePositions(doorFramePositions).Contains(interactAt))
            {
                interactor.AddInteraction(new DialogLine(interactor, "It's locked"));

                Key key = (Key)interactor.parent.inventory.Where(CorrectKey).FirstOrDefault()?.item;
                if (key != null)
                {
                    interactor.AddInteraction(
                        new Choice(interactor,
                            "Unlock?",
                            new Choice.Option(
                                "yes",
                                new CustomAction(
                                    interactor,
                                    i =>
                                    {
                                        Unlock();
                                        i.parent.inventory.Remove(key);
                                    })
                                ),
                            new Choice.Option("no")
                            )
                       );
                }

                return true;
            }
            else return false;
        }

        bool CorrectKey(ItemBag.ItemCount key)
            => (key.item as Key)?.doorName == name;

        public bool OnDoorMat(Vector2i pos)
            => TransformRelativeTilePositions(doorMatPositions).Contains(pos);

        public void Lock()
            => locked = true;

        public void Unlock()
            => locked = false;
    }
}
