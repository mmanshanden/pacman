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

        public Vector2 Target
        {
            get;
            set;
        }

        public States State
        {
            get;
            private set;
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
            Vector2 times = Collision.IntersectionTime(this.Center, this.Velocity, this.Target);
            float time = Collision.GetSmallestPositive(times.X, times.Y);

            if (time < dt)
            {
                Console.WriteLine("Target event");
                this.Center = this.Target;
                this.Collision_Target(board, tile);
            }
            else
            {
                time = 0;
            }
            
            base.Move(board, tile, dt - time);
        }
    }
}
