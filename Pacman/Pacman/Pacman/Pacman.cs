using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pacman : GameCharacter
    {
        public int Lives
        {
            get;
            set; 
        }

        public int Score
        {
            get;
            set; 
        }

        private Vector2 spawn;

        public Vector2 Spawn
        {
            get { return this.spawn; }
            set
            {
                this.Position = value;
                this.spawn = value;
            }
        }

        public Pacman()
        {
            this.Lives = 3;
            this.Score = 0; 
        }

        public override void Collision_GameObject(GameObject gameObject)
        {
            if (gameObject is Bubble)
                this.Score++;
                
            Ghost ghost = gameObject as Ghost;
            if (ghost != null)
            {
                if (ghost.State == Ghost.States.Chase || ghost.State == Ghost.States.Scatter)
                {
                    this.Dead(); 
                }
            }
        }

        public virtual void Dead()
        {
            this.Lives--;
            this.Position = this.spawn;
            Level level = (Level)this.Parent;
            level.GhostHouse.ResetGhosts(); 
        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Yellow);
            drawHelper.Translate(-this.Position);
        }
    }
}