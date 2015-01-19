using Base;
using Microsoft.Xna.Framework;
using Network;

namespace Pacman
{
    class Player : Pacman
    {
        private Vector2 direction;
        private Vector2 queued;

        // New directions are queued first. Only when the
        // player reaches a junction, the newly added will
        // become the active direction. 
        public override Vector2 Direction
        {
            get
            {
                // current direction
                return this.direction;
            }
            set
            {
                // limit value to up, down, left and right
                value = Collision.ToDirectionVector(value);

                // immediately overwrite if our direction is 0
                if (this.direction.X == 0 && this.direction.Y == 0)
                    this.direction = value;

                // invalid value, queue current direction
                if (value == Vector2.Zero)
                {
                    this.queued = this.direction;
                    return;
                }

                this.queued = value;

                // player is allowed to reverse direction at all times
                if (this.queued * -1 == this.direction)
                {
                    this.direction = queued;
                }
            }
        }

        public override void Collision_InvalidDirection(GameBoard gameBoard, GameTile tile)
        {
            this.direction = Vector2.Zero;
        }

        public override void Collision_Junction(GameBoard gameBoard, GameTile tile)
        {
            // position of next tile 
            Vector2 p = this.Position + this.queued;

            if (!gameBoard.GetTile(p).IsCollidable(this))
                this.direction = queued;
        }

        public override void Dead()
        {
            this.direction = Vector2.Zero;
            base.Dead();
        }


        public void HandleInput(InputHelper inputHelper)
        {
            this.Direction = inputHelper.GetDirectionalInput();
        }
    }
}
