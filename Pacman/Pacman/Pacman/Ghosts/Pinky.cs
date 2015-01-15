using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pinky : Ghost
    {
        private Pacman pacman;

        public Pinky(Pacman pacman)
            : base()
        {
            this.pacman = pacman;
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override Vector2 GetTarget(Ghost.States state)
        {
            if (state == States.Chase)
                return this.Target = pacman.Center + pacman.Direction * 4; 

            return base.GetTarget(state);
        }

        

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.State != States.Chase && this.State != States.Scatter)
            {
                base.Draw(drawHelper);
                return; 
            }

            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Pink);
            drawHelper.Translate(-this.Position);
        }
    }
}
