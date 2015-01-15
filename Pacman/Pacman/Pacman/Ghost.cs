using Base;
using Microsoft.Xna.Framework;
namespace Pacman
{
    class Ghost : GameCharacter
    {
        public enum States
        {
            Chase,
            Scatter,
            Dead,
        }

        public GhostHouse GhostHouse
        {
            get
            {
                return this.Parent as GhostHouse;
            }
        }

        public Vector2 Target
        {
            get;
            set;
        }

        public States State
        {
            get;
            set;
        }

        public Ghost()
        {
            this.State = States.Chase;
        }

        #region Collision Events
        public override void Collision_InvalidDirection(GameBoard board, GameTile tile)
        {
            if (tile.GetNeighbourCount(this) == 1)
                this.Direction = Vector2.Zero;

            this.Collision_Junction(board, tile);
        }
        public override void Collision_Junction(GameBoard board, GameTile tile)
        {
            Vector2 move = this.Direction;
            float min = float.PositiveInfinity;

            foreach (GameTile neighbour in tile.GetNeighbourList(this))
            {
                Vector2 direction = neighbour.Position - tile.Position;

                // ghosts never go back, so we skip that direction
                if (direction * -1 == this.Direction)
                    continue;

                Vector2 center = neighbour.Center;
                float distance = Vector2.Distance(center, this.Target);

                if (distance < min)
                {
                    min = distance;
                    move = direction;
                }

            }

            this.Direction = move;
        }
        public virtual void Collision_Target(GameBoard board, GameTile tile)
        {

        }
        #endregion

        protected override void Move(GameBoard board, GameTile tile, float dt)
        {
            float v, j, p;

            // set velocity, junction, position
            // and next tile based on direction.
            if (this.Target.Y == this.Center.Y)
            {
                v = this.Velocity.X;
                p = this.Center.X;
                j = this.Target.X;
            }
            else if (this.Target.X == this.Center.X)
            {
                v = this.Velocity.Y;
                p = this.Center.Y;
                j = this.Target.Y;
            }
            else
            {
                base.Move(board, tile, dt);
                return;
            }

            float t = Collision.SolveForX(v, p, j);

            // will we reach target?
            if (t < 0 || t > dt)
            {
                // nope..!

                base.Move(board, tile, dt);
                return;
            }

            // we crossed junction
            this.Center = this.Target;
            this.Collision_Target(board, tile);
            Console.WriteLine("Target event");
            base.Move(board, tile, dt - t);
        }
    }
}
