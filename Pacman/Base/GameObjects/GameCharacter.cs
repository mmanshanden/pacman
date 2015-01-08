using Microsoft.Xna.Framework;

namespace Base
{
    public class GameCharacter : GameObject
    {
        public virtual Vector2 Direction
        {
            get;
            set;
        }
        public float Speed
        {
            get;
            set;
        }
        public Vector2 Velocity
        {
            get { return this.Direction * this.Speed; }
        }
        protected GameWorld World
        {
            get { return this.Parent as GameWorld; }
        }

        protected Vector2 Center
        {
            get { return this.Position + Vector2.One * 0.5f; }
        }

        public GameCharacter()
        {
            this.Direction = Vector2.Zero;
            this.Speed = 1;
        }

        public void Move(GameBoard board, float dt)
        {
            Vector2 velocity = this.Velocity;
            Vector2 center = this.Center;

            if (velocity == Vector2.Zero)
                return;

            if (board == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            GameTile tile, next;
            tile = next = board.Get(center);

            Vector2 junction = tile.Center;

            float v, p, j;

            if (velocity.X != 0)
            {
                v = velocity.X;
                p = center.X;
                j = junction.X;

                if (velocity.X > 0)
                    next = tile.Right;
                else
                    next = tile.Left;
            }
            else
            {
                v = velocity.Y;
                p = center.Y;
                j = junction.Y;

                if (velocity.Y > 0)
                    next = tile.Bottom;
                else
                    next = tile.Top;
            }

            float t = Collision.SolveForX(v, p, j);

            if (t < 0 || t > dt)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            this.Position = tile.Position;

            if (next.Collidable)
            {
                this.Collision_InvalidDirection(board);
                this.Position += this.Velocity * (dt - t);
                return;
            }

            this.Position += this.Velocity * (dt - t);
        }

        public virtual void Collision_InvalidDirection(GameBoard board)
        {
            this.Direction = Vector2.Zero;
        }
        public virtual void Collision_Junction(GameBoard board)
        {

        }

    }
}
