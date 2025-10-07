using System.Linq;
using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosUtil.Primitives;
using SysCol = System.Collections.Generic;
using static ChaosFramework.Math.Clamping;

namespace LD58.World.Objectives
{
    using ChaosFramework.Collections;
    using Interaction.Steps;
    using Objects;
    using Objects.WorldObjects;
    using Player;

    class DoWork
        : Objective
    {
        class UserInteractionStatus
        {
            public int superLazyThought = 0;
            public int lazyThought = 0;
            public int almostDoneThought = 0;
            public int doneThought = 0;
        }

        static readonly string[] SUPER_LAZY_THOUGHTS = new[] {
            "I haven't done a thing yet.",
            "Ugh, but it's such boring work.",
            "I really, really don't want to do this.",
            "...",
            "Nobody's here anyways.",
            "...",
            "...",
            "...",
            "...",
            "...",
            "Screw it, it's break time!",
        };

        static readonly string[] LAZY_THOUGHTS = new[] {
            "I've already started, gotta commit now!",
            "There's still so much to do...",
            "I could really use a break already.",
            "Ugh. It's not even near break time yet.",
            "Why do I have to do everyone's work anyways?",
            "I should get back to work...",
        };

        static readonly string[] ALMOST_DONE_THOUGHTS = new[] {
            "I'm almost done, then it's time for a break!",
            "Just a few more work items to go!",
            "...",
            "I can't do this anymore...",
            "... I need to get back to work.",
        };

        static readonly string[] DONE_THOUGHTS = new[] {
            "I've gotten enough done for now.",
            "Hmm... maybe I can do one or two more things",
            "It'd be fine to take a break, but my mind is still buzzing with work things.",
            "Eh, whatever, I've done enough and deserve this break!",
        };

        static readonly string SUPER_DONE_THOUGHT = "Wowiee, I have done, like, the work for five whole days just now!\nI really need a break...";

        const int REQUIRED_WORK_ITEMS = 15;

        SysCol.Dictionary<Interactor, UserInteractionStatus> interactionStatus = new SysCol.Dictionary<Interactor, UserInteractionStatus>();

        SysCol.HashSet<OfficeTable> freeTables = new SysCol.HashSet<OfficeTable>();
        float newWorkTimer, speed;
        bool finished;

        int progress;

        DoorFrame breakRoomDoor, bossOfficeDoor;

        protected override string GetText()
            => $"Do work!\n[{new string('=', progress)}{new string(' ', Max(0, REQUIRED_WORK_ITEMS - progress))}{new string('\b', Max(0, progress - REQUIRED_WORK_ITEMS))}]";

        protected override void Create(CreateParameters cparams)
        {
            base.Create(cparams);
            foreach (WorldObject obj in scene.EnumerateChildren<WorldObject>(false))
            {
                OfficeTable table = obj as OfficeTable;
                if (table != null)
                    freeTables.Add(table);

                DoorFrame door = obj as DoorFrame;
                if (door?.GetName() == "Break Room")
                    breakRoomDoor = door;
                else if (door?.GetName() == "Boss Office")
                    bossOfficeDoor = door;
            }

            newWorkTimer = 2;
            speed = 1;

            breakRoomDoor.Lock();
        }

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();
            scene.updateLayers[(int)UpdateLayers.ObjectiveLogic].Add(Update);
        }

        public override bool Interact(Interactor interactor, Interactible interactible, Vector2i interactAt)
        {
            UserInteractionStatus userStatus = interactionStatus.GetOrCreateValue(interactor);

            OfficeTable table = interactible as OfficeTable;
            if (table != null)
            {
                WorkItem work = table.EnumerateChildren<WorkItem>(false).SingleOrDefault();
                if (work != null && table.FacingScreenZero(interactor.parent.direction) ^ work.isRightSide)
                    interactor.AddInteraction(
                        new DialogLine(interactor, "Another off by one error."),
                        new CustomAction(interactor, (Interactor _) =>
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

            if (interactible == breakRoomDoor)
            {
                if (progress == 0)
                {
                    if (userStatus.superLazyThought == SUPER_LAZY_THOUGHTS.Length - 1)
                        interactor.AddInteraction(
                            new DialogLine(interactor, SUPER_LAZY_THOUGHTS[userStatus.superLazyThought]),
                            new CustomAction(interactor, Complete)
                            );
                    else
                        interactor.AddInteraction(new DialogLine(interactor, SUPER_LAZY_THOUGHTS[userStatus.superLazyThought++]));
                }
                else if (progress < REQUIRED_WORK_ITEMS / 2)
                {
                    interactor.AddInteraction(new DialogLine(interactor, LAZY_THOUGHTS[userStatus.lazyThought]));
                    userStatus.lazyThought = Min(userStatus.lazyThought + 1, LAZY_THOUGHTS.Length - 1);
                }
                else if (progress < REQUIRED_WORK_ITEMS)
                {
                    interactor.AddInteraction(new DialogLine(interactor, ALMOST_DONE_THOUGHTS[userStatus.almostDoneThought]));
                    userStatus.almostDoneThought = Min(userStatus.almostDoneThought + 1, ALMOST_DONE_THOUGHTS.Length - 1);
                }
                else if (progress < REQUIRED_WORK_ITEMS * 5)
                {
                    if (userStatus.doneThought == DONE_THOUGHTS.Length - 1)
                        interactor.AddInteraction(
                            new DialogLine(interactor, DONE_THOUGHTS[userStatus.doneThought]),
                            new CustomAction(interactor, Complete)
                            );
                    else
                        interactor.AddInteraction(
                        new Choice(interactor, DONE_THOUGHTS[userStatus.doneThought++],
                        new Choice.Option("Take a break",
                            new DialogLine(interactor, "Time to wind down a bit. Let's see..."),
                            new CustomAction(interactor, Complete)
                            ),
                        new Choice.Option("Continue work")
                        ));
                }
                else // done way too much work lol
                    interactor.AddInteraction(
                        new DialogLine(interactor, SUPER_DONE_THOUGHT),
                        new CustomAction(interactor, Complete)
                        );

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
        {
            breakRoomDoor.Unlock();
            bossOfficeDoor.Unlock();
            foreach (WorkItem workItem in scene.EnumerateChildren<WorkItem>(true))
                workItem.Complete();

            scene.SetObjective<LunchBreak>();
        }
    }
}
