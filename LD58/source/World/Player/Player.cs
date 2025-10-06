using ChaosFramework.Collections;
using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;
using System.Linq;
using static ChaosFramework.Math.Exponentials;

namespace LD58.World.Player
{
    using Objects;

    [DefaultInstancer(64, "objects/Player.gmdl", "objects/Bathroom.mat")]
    public class Player
        : WorldObject
    {
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down,
        }

        const float WALKING_INTERVAL = 0.1f;
        const float CAMERA_LAG_DISTANCE = 1.5f;

        static Vector2i MapDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return new Vector2i(0, 1);
                case Direction.Down: return new Vector2i(0, -1);
                case Direction.Left: return new Vector2i(-1, 0);
                case Direction.Right: return new Vector2i(1, 0);
                default: throw new System.Exception($"'{direction}' does not represent a direction!");
            }
        }

        static bool AreOpposite(Direction a, Direction b)
        {
            switch (a)
            {
                case Direction.Up: return b == Direction.Down;
                case Direction.Down: return b == Direction.Up;
                case Direction.Left: return b == Direction.Right;
                case Direction.Right: return b == Direction.Left;
                default: return false;
            }
        }

        public Interactor interactor { get; private set; }
        public PlayerInventory inventory { get; private set; }

        public Vector2i position;
        public Vector2i direction;

        Direction facing;
        float walkingHowLong = float.NaN;
        Vector2f visualPosition;

        Vector2f cameraTarget;

        // TODO: Capture actual input instead of just direction,
        //       to support multiple keys (or keyboards) for the same action
        LinkedList<Direction> inputs = new LinkedList<Direction>();

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            position = OccupiedTiles().First();
            direction = new Vector2i(bone.GetDirection().xz);
            visualPosition = position;

            interactor = AddComponent<Interactor>();
            inventory = AddComponent<PlayerInventory>();
        }

        public override bool CanStepOn(Vector2i pos)
            => true;

        public override void SetUpdateCalls()
        {
            base.SetUpdateCalls();

            scene.game.input.AddHandler<InputPushEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Move, KeyDown);
            scene.game.input.AddHandler<InputReleaseEvent<Keyboard.Key>, Keyboard.Key>(InputLayers.Move, KeyUp);

            scene.updateLayers[(int)UpdateLayers.PlayerMove].Add(Move);
            scene.updateLayers[(int)UpdateLayers.UpdateCamera].Add(UpdateView);
        }

        void UpdateView()
        {
            if (visualPosition.x > cameraTarget.x + CAMERA_LAG_DISTANCE)
                cameraTarget.x = visualPosition.x - CAMERA_LAG_DISTANCE;
            if (visualPosition.x < cameraTarget.x - CAMERA_LAG_DISTANCE)
                cameraTarget.x = visualPosition.x + CAMERA_LAG_DISTANCE;

            if (visualPosition.y > cameraTarget.y + CAMERA_LAG_DISTANCE)
                cameraTarget.y = visualPosition.y - CAMERA_LAG_DISTANCE;
            if (visualPosition.y < cameraTarget.y - CAMERA_LAG_DISTANCE)
                cameraTarget.y = visualPosition.y + CAMERA_LAG_DISTANCE;

            Vector3f target = new Vector3f(cameraTarget.x + 0.5f, 1, cameraTarget.y + 0.5f);
            Vector3f direction = new Vector3f(0, -1, 0);
            Vector3f pos = target - direction * 10;
            scene.view.Update(pos, direction, new Vector3f(0, 0, 1));
        }

        bool KeyDown(InputPushEvent<Keyboard.Key> e)
        {
            if (!interactor.busy)
                switch (e.axis.key)
                {
                    case Keyboard.Keys.W: return StartWalking(Direction.Up);
                    case Keyboard.Keys.A: return StartWalking(Direction.Left);
                    case Keyboard.Keys.S: return StartWalking(Direction.Down);
                    case Keyboard.Keys.D: return StartWalking(Direction.Right);
                    case Keyboard.Keys.Space: return Interact();
                }

            return false;
        }

        bool KeyUp(InputReleaseEvent<Keyboard.Key> e)
        {
            if (!interactor.busy)
                switch (e.axis.key)
                {
                    case Keyboard.Keys.W: return StopWalking(Direction.Up);
                    case Keyboard.Keys.A: return StopWalking(Direction.Left);
                    case Keyboard.Keys.S: return StopWalking(Direction.Down);
                    case Keyboard.Keys.D: return StopWalking(Direction.Right);
                }

            return false;
        }

        bool StartWalking(Direction direction)
        {
            inputs.Insert(0, direction);

            if (float.IsNaN(walkingHowLong))
            {
                // start walking
                walkingHowLong = 0;

                // allow tapping for a single step
                if (facing == direction)
                    Step();
            }

            TurnTo(direction);
            return true;
        }

        bool StopWalking(Direction direction)
        {
            if (inputs.Remove(direction))
            {
                if (inputs.empty)
                    walkingHowLong = float.NaN;
                else
                    TurnTo(inputs.first);
            }

            return false;
        }

        public void FullStop()
        {
            inputs.Clear();
            walkingHowLong = float.NaN;
        }

        void TurnTo(Direction direction)
        {
            facing = direction;
            this.direction = MapDirection(direction);
        }

        bool Interact()
        {
            Interactible interactible = scene[position + direction] as Interactible;
            if (interactible != null && scene.objective.Interact(interactor, interactible))
            {
                FullStop();
                return true;
            }
            else
                return false;
        }

        void Move()
        {
            if (!float.IsNaN(walkingHowLong))
                if ((walkingHowLong += ftime) > WALKING_INTERVAL)
                {
                    walkingHowLong -= WALKING_INTERVAL;
                    TurnTo(facing);
                    Step();
                }

            visualPosition += (position - visualPosition) * EaseIn(ftime * 10);
        }

        void Step()
        {
            if (!TryStep(facing))
            {
                foreach (Direction dir in inputs)
                    if (dir == facing)
                        continue;
                    else if (!AreOpposite(facing, dir))
                    {
                        TryStep(dir);
                        return;
                    }
            }
        }

        bool TryStep(Direction d)
        {
            Vector2i desiredPos = position + MapDirection(d);
            if (scene.CanEnter(desiredPos))
            {
                position = desiredPos;
                return true;
            }
            else
                return false;
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
            => instancers[0].informer.AddInstance(
                new Matrix(
                    direction.y, 0, -direction.x, 0,
                    0, 1, 0, 0,
                    direction.x, 0, direction.y, 0,
                    visualPosition.x + 0.5f, 0, visualPosition.y + 0.5f, 1
                    )
                );
    }
}
