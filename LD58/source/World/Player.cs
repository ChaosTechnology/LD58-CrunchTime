using ChaosFramework.Components;
using ChaosFramework.Graphics.OpenGl.Instancing;
using ChaosFramework.Input.InputEvents;
using ChaosFramework.Input.RawInput;
using ChaosFramework.Math;
using ChaosFramework.Math.Vectors;
using System.Linq;

namespace LD58.World
{
    using Objects;

    [DefaultInstancer(64, "objects/Player.gmdl", "objects/Bathroom.mat")]
    public class Player
        : WorldObject
    {
        public enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down,
        }

        const float WALKING_INTERVAL = 0.1f;

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

        public Vector2i position;
        public Vector2i direction;

        Direction walkingTo;
        float walkingHowLong = 0;

        protected override void Create(CreateParameters args)
        {
            base.Create(args);
            position = OccupiedTiles().First();
            direction = new Vector2i(bone.GetDirection().xz);
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
            Vector3f target = new Vector3f(position.x, 1, position.y);
            Vector3f direction = new Vector3f(0, -1, 0);
            Vector3f pos = target - direction * 10;
            scene.view.Update(pos, direction, new Vector3f(0, 0, 1));
        }

        bool KeyDown(InputPushEvent<Keyboard.Key> e)
        {
            switch (e.axis.key)
            {
                case Keyboard.Keys.W: return StartWalking(Direction.Up);
                case Keyboard.Keys.A: return StartWalking(Direction.Left);
                case Keyboard.Keys.S: return StartWalking(Direction.Down);
                case Keyboard.Keys.D: return StartWalking(Direction.Right);
                default: return false;
            }
        }

        bool KeyUp(InputReleaseEvent<Keyboard.Key> e)
        {
            switch (e.axis.key)
            {
                case Keyboard.Keys.W: return StopWalking(Direction.Up);
                case Keyboard.Keys.A: return StopWalking(Direction.Left);
                case Keyboard.Keys.S: return StopWalking(Direction.Down);
                case Keyboard.Keys.D: return StopWalking(Direction.Right);
                default: return false;
            }
        }

        bool StartWalking(Direction direction)
        {
            walkingTo = direction;
            walkingHowLong = 0;
            return true;
        }

        bool StopWalking(Direction direction)
        {
            if (direction == walkingTo)
                TurnTo(direction);

            walkingTo = Direction.None;
            walkingHowLong = 0;

            return false;
        }

        void TurnTo(Direction direction)
        {
            this.direction = MapDirection(direction);
        }

        void Move()
        {
            if (walkingTo != Direction.None)
                if ((walkingHowLong += ftime) > WALKING_INTERVAL)
                {
                    walkingHowLong -= WALKING_INTERVAL;
                    TurnTo(walkingTo);
                    Vector2i desiredPos = position + MapDirection(walkingTo);
                    if (scene.CanEnter(desiredPos))
                        position = desiredPos;
                }
        }

        public override void GiveMeInstances(InstancingAttribute[] instancers)
            => instancers[0].informer.AddInstance(
                new Matrix(
                    direction.y, 0, -direction.x, 0,
                    0, 1, 0, 0,
                    direction.x, 0, direction.y, 0,
                    position.x + 0.5f, 0, position.y + 0.5f, 1
                    )
                );
    }
}
