using System.Linq;
using ChaosFramework.Collections;
using ChaosFramework.Components;
using ChaosFramework.Math.Vectors;
using ChaosUtil.Primitives;
using static ChaosFramework.Math.Clamping;
using SysCol = System.Collections.Generic;

namespace LD58.World.Objectives
{
    using Interaction;
    using Interaction.Steps;
    using Constants;
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

        static readonly string[] NORMAL_WORK_TASKS = new[]
        {
            "Yet another off-by-one error.\nHow boring...",
            "Seems like this document is malformed.\nWho wrote this parser anyways?",
            "This code looks like it was written by a smashed pavian!",
            "I'm just gonna delete this...\nHopefully no one uses this feature.",
            "Soooo much spaghetti code...",
            "This bug fix probably introduces 5 new bugs in the process!",
        };

        static readonly string[][] URGENT_WORK_TASKS = new[]
        {
            new[]
            {
                "WHAT THE HECK IS THIS?!",
                "They just put a DELETE statement there?!?\nThat's hundreds of data sets DESTROYED!",
                "That'll do some serious damage to our reputation!",
            },
            new[]
            {
                "WHO IS RESPONSIBLE FOR THIS PILE OF GARBAGE?!",
                "A toddler could write better code than this just by screaming around!",
                "The cleanup for this will take forever!",
            },
            new[]
            {
                "IDIOTS! IDIOTS! IDIOTS!!!",
                "Learn the damn difference between a set and a list!\nThey're different things for a reason!",
                "Now I have to write ANOTHER duplicate elimination routine\nthat'll waste CPU for absolutely no gain!",
            },
            new[]
            {
                "WHYYYYYYYYYYYYY???",
                "Who thought it was a good idea to connect to the database...\nFOR EVERY SINGLE ITEM!",
                "No wonder that we need to restock on hardware every other month!",
            }
        };

        protected virtual int required => 15;
        protected virtual string[] superLazyThoughts => SUPER_LAZY_THOUGHTS;
        protected virtual string[] lazyThoughts => LAZY_THOUGHTS;
        protected virtual string[] almostDoneThoughts => ALMOST_DONE_THOUGHTS;
        protected virtual string[] doneThoughts => DONE_THOUGHTS;
        protected virtual string superDoneThought => SUPER_DONE_THOUGHT;
        protected virtual string failureThought => "Might as well take a break.";
        protected virtual string endOption => "Take a break";
        protected virtual string endThought => "Time to wind down a bit. Let's see...";

        SysCol.Dictionary<Interactor, UserInteractionStatus> interactionStatus = new SysCol.Dictionary<Interactor, UserInteractionStatus>();

        SysCol.HashSet<OfficeTable> freeTables = new SysCol.HashSet<OfficeTable>();
        float newWorkTimer, speed;
        protected bool failure { get; private set; }

        int progress;

        DoorFrame bossOfficeDoor;
        protected readonly SysCol.HashSet<DoorFrame> objectiveEndingDoors = new SysCol.HashSet<DoorFrame>();

        protected override string GetText()
            => $"Do work!\n[{new string('=', progress)}{new string(' ', Max(0, required - progress))}{new string('\b', Max(0, progress - required))}]";

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
                    objectiveEndingDoors.Add(door);
                else if (door?.GetName() == "Boss Office")
                    bossOfficeDoor = door;
            }

            newWorkTimer = 2;
            speed = 1;

            foreach (DoorFrame door in objectiveEndingDoors)
                door.Lock();
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
                if (work != null && table.FacingScreenZero(interactor.parent.direction) ^ work.isRightSide && !work.Done())
                {
                    SysCol.IEnumerable<InteractionStep> dialogLines = work.Urgent()
                        ? URGENT_WORK_TASKS.RandomElement().Select(x => new DialogLine(interactor, x))
                        : Util.Yield(new DialogLine(interactor, NORMAL_WORK_TASKS.RandomElement()));
                    interactor.AddInteraction(
                        dialogLines.Concat(Util.Yield(
                            new CustomAction(
                                interactor,
                                (Interactor _) =>
                                {
                                    interactor.parent.inventory.AddItem(work.Urgent() ? KnownItems.URGENT_WORK_ITEM : KnownItems.WORK_ITEM);
                                    work.Complete();
                                    progress++;
                                    freeTables.Add(table);
                                })
                            ))
                        );
                }
                else if (table.FacingAnyScreen(interactor.parent.direction))
                    interactor.AddInteraction(
                        new DialogLine(interactor, "This isn't pretty, but it doesn't need my immediate attention.")
                        );
                else
                    return false;

                return true;
            }

            if (objectiveEndingDoors.Contains(interactible))
            {
                if (progress == 0)
                {
                    if (userStatus.superLazyThought == superLazyThoughts.Length - 1)
                        interactor.AddInteraction(
                            new DialogLine(interactor, superLazyThoughts[userStatus.superLazyThought]),
                            new CustomAction(interactor, Complete)
                            );
                    else
                        interactor.AddInteraction(new DialogLine(interactor, superLazyThoughts[userStatus.superLazyThought++]));
                }
                else if (progress < required / 2)
                {
                    interactor.AddInteraction(new DialogLine(interactor, lazyThoughts[userStatus.lazyThought]));
                    userStatus.lazyThought = Min(userStatus.lazyThought + 1, lazyThoughts.Length - 1);
                }
                else if (progress < required)
                {
                    interactor.AddInteraction(new DialogLine(interactor, almostDoneThoughts[userStatus.almostDoneThought]));
                    userStatus.almostDoneThought = Min(userStatus.almostDoneThought + 1, almostDoneThoughts.Length - 1);
                }
                else if (progress < required * 5)
                {
                    if (userStatus.doneThought == doneThoughts.Length - 1)
                        interactor.AddInteraction(
                            new DialogLine(interactor, doneThoughts[userStatus.doneThought]),
                            new CustomAction(interactor, Complete)
                            );
                    else
                        interactor.AddInteraction(
                        new Choice(interactor, doneThoughts[userStatus.doneThought++],
                        new Choice.Option(endOption,
                            new DialogLine(interactor, endThought),
                            new CustomAction(interactor, Complete)
                            ),
                        new Choice.Option("Continue work")
                        ));
                }
                else // done way too much work lol
                    interactor.AddInteraction(
                        new DialogLine(interactor, superDoneThought),
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
                    if (!failure)
                        foreach (Interactor interactor in scene.EnumerateChildren<Interactor>(true))
                            interactor.AddInteraction(
                                new DialogLine(interactor, "Welp, everthing is broken now..."),
                                new DialogLine(interactor, failureThought),
                                new CustomAction(interactor, Complete)
                                );

                    failure = true;
                }
                else
                {
                    chosen.AddComponent<WorkItem>(CreateParameters.Create(chosen.bone, Random.instance.RndBool()));
                    freeTables.Remove(chosen);
                    newWorkTimer = 5;
                }
            }
        }

        protected virtual void Complete(Interactor _)
        {
            ConcludeWork(!failure);
            scene.SetObjective<LunchBreak>();
        }

        protected void ConcludeWork(bool completeWorkItems)
        {
            foreach (DoorFrame door in objectiveEndingDoors)
                door.Unlock();

            bossOfficeDoor.Unlock();

            if (!failure)
                foreach (WorkItem workItem in scene.EnumerateChildren<WorkItem>(true))
                    workItem.Complete();
        }
    }
}
