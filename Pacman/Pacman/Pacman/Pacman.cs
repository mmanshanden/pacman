using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pacman : GameObject
    {
        private Vector2 direction;
        private Vector2 queued;

        public override Vector2 Direction
        {
            get
            {
                return this.direction;
            }
            set
            {
                if (this.direction.X == 0 && this.direction.Y == 0)
                {
                    this.direction = value;
                }

                if (value.X == 0 && value.Y == 0)
                {
                    this.queued = this.direction;
                    return;
                }

                this.queued = value;

                if (this.queued * -1 == this.direction)
                {
                    this.direction = queued;
                }
            }
        }


        public Pacman()
        {

        }

        public override void Collision_InvalidDirection(GameBoard gameBoard)
        {
            this.direction = Vector2.Zero;
        }

        public override void Collision_Junction(GameBoard gameBoard)
        {
            Point tile = this.Tile;
            tile.X += (int)this.queued.X;
            tile.Y += (int)this.queued.Y;

            if (!gameBoard.IsCollidable(tile))
                this.direction = queued;
        }
    }
}
