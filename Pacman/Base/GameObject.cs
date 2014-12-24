using Microsoft.Xna.Framework;

namespace Base
{
    public class GameObject : ILoopMember
    {
        public Vector2 Position
        {
            get;
            set;
        }
        public Vector2 Center
        {
            get
            {
                return this.Position + Vector2.One * 0.5f;
            }
            set
            {
                this.Position = value - Vector2.One * 0.5f;
            }
        }
        public Point Tile
        {
            get
            {
                return Collision.ToPoint(this.Center);
            }
        }

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
            get
            {
                return this.Direction * this.Speed;
            }
        }

        public GameObject()
        {

        }

        public bool CollidesWith(GameObject gameObject)
        {
            return (gameObject.Tile == this.Tile);
        }

        #region Collision Events
        public virtual void Collision_GameObject(GameObject gameObject)
        {

        }

        public virtual void Collision_InvalidDirection(GameBoard gameBoard)
        {
            this.Direction = Vector2.Zero;
        }

        public virtual void Collision_InvalidPosition()
        {

        }

        public virtual void Collision_Junction(GameBoard gameBoard)
        {

        }
        #endregion

        public virtual void Move(float dt, GameBoard gameBoard)
        {
            if (this.Velocity == Vector2.Zero)
                return;

            if (gameBoard == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            Vector2 junction = Collision.TileCenter(this.Tile);
            Point next = this.Tile;

            float v, p, j;

            if (this.Velocity.X != 0)
            {
                v = this.Velocity.X;
                p = this.Center.X;
                j = junction.X;

                next.X += (int)this.Direction.X;
            }
            else
            {
                v = this.Velocity.Y;
                p = this.Center.Y;
                j = junction.Y;

                next.Y += (int)this.Direction.Y;
            }

            float t = Collision.SolveForX(v, p, j);

            if (t < 0 || t > dt)
            {
                this.Position += this.Velocity * dt;
                return;
            }
            
            this.Center = junction;

            if (gameBoard.IsCollidable(next))
            {
                this.Collision_InvalidDirection(gameBoard);
                this.Position += this.Velocity * (dt - t);
                return;
            }

            switch (gameBoard.GetNeighbourCount(this))
            {
                case 0:
                    this.Collision_InvalidPosition();
                    break;
                case 3:
                case 4:
                    this.Collision_Junction(gameBoard);
                    break;
            }
            
            this.Position += this.Velocity * (dt - t);
        }

        public virtual void Update(float dt)
        {
            
        }

        public virtual void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Black);
            drawHelper.Translate(-this.Position);
        }
        
    }
}
