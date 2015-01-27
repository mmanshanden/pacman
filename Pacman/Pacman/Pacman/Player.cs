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

        // If player can't go further that way we set his direction to Zero.
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

        // Check whether we collide with certain object
        public override void Collision_GameObject(GameObject gameObject)
        {
            if (gameObject is Bubble)
            {
                Game.SoundManager.PlaySoundEffect("bubble");
                // Set speed to 0 to make ghosts able to catch up
                this.Speed = 0;
            }                
            
            base.Collision_GameObject(gameObject);
        }

        public override void Die()
        {
            // Set direction to Zero when there is collision with a ghost that isn't frightened
            this.direction = Vector2.Zero;

            base.Die();
        }

        public override void Update(float dt)
        {
            //  Zoom out and show map from above when player is dead
            if (this.Lives > 0)
                Game.Camera.Target = this.Center + Vector2.UnitY * 2;
            else
            {
                Game.Camera.Rho = MathHelper.PiOver2 - 0.08f;
                Game.Camera.Zoom = 26;
            }

            base.Update(dt);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.Direction = inputHelper.GetDirectionalInput();
        }

        public override void Draw(DrawHelper drawHelper)
        {
            // Draw the score on the screen
            drawHelper.DrawString(this.Score.ToString(), new Vector2(0.98f, 0.02f), DrawHelper.Origin.TopRight, Color.White);

            drawHelper.DrawString(this.Lives.ToString(), new Vector2(0.02f, 0.02f), DrawHelper.Origin.TopLeft, Color.White);
        }
    }
}