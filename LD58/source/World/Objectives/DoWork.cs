using System.Linq;
using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;
using static ChaosFramework.Math.Clamping;

namespace LD58.World.Objectives
{
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class DoWork
        : Objective
    {
        const int REQUIRED_WORK_ITEMS = 15;

        SysCol.HashSet<OfficeTable> freeTables = new SysCol.HashSet<OfficeTable>();
        float newWorkTimer, speed;
        bool finished;

        int progress;

        protected override string GetText()
            => $"Do work!\n[{new string('=', progress)}{new string(' ', Max(0, REQUIRED_WORK_ITEMS - progress))}{new string('\b', Max(0, progress - REQUIRED_WORK_ITEMS))}]";

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            for (Vector2i pos = 0; pos.x < scene.size.x; pos.x++)
                for (pos.y = 0; pos.y < scene.size.y; pos.y++)
                    freeTables.Add(scene[pos] as OfficeTable);

            newWorkTimer = 2;
            speed = 1;
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            scene.updateLayers[(int)UpdateLayers.ObjectiveLogic].Add(Update);
        }

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            OfficeTable table = interactible as OfficeTable;
            if (table != null)
            {
                WorkItem work = table.EnumerateChildren<WorkItem>(false).SingleOrDefault();
                if (work != null && table.FacingScreenZero(interactor.parent.direction) ^ work.isRightSide)
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Another off by one error."),
                        new CustomAction(interactor, () =>
                        {
                            work.Complete();
                            progress++;
                            freeTables.Add(table);
                        })
                        );
                else if (table.FacingAnyScreen(interactor.parent.direction))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "This isn't pretty, but it doesn't need my immediate attention.")
                        );
                else
                    return false;

                return true;
            }

            return base.Interact(interactor, interactible, interactAt);
        }

        void Update()
        {
            speed += ftime * 0.025f;
            if ((newWorkTimer -= ftime * speed) < 0)
            {
                OfficeTable chosen = null;
                int i = 0;
                int chosenIndex = Random.instance.RndInt(freeTables.Count);
                foreach (OfficeTable table in freeTables)
                    if (i++ == chosenIndex)
                    {
                        chosen = table;
                        break;
                    }

                if (chosen == null) // everything is broken
                {
                    if (!finished)
                        foreach (Interactor interactor in scene.EnumerateChildren<Interactor>(true))
                            interactor.AddInteraction(
                                new DialogLine(interactor, "Welp, everthing is broken now..."),
                                new DialogLine(interactor, "Might as well take a break."),
                                new CustomAction(interactor, Complete)
                                );

                    finished = true;
                }
                else
                {
                    chosen.AddComponent<WorkItem>(CreateParameters.Create(chosen.bone, Random.instance.RndBool()));
                    freeTables.Remove(chosen);
                    newWorkTimer = 5;
                }
            }
        }

        void Complete()
            => scene.SetObjective<Hygiene>();
    }
}
