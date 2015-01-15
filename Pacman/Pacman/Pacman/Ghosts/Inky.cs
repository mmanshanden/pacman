using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Inky : Ghost
    {
        private Pacman pacman;
        private Blinky blinky;

        public Inky(Pacman pacman, Blinky blinky)
            : base()
        {
            this.pacman = pacman;
            this.blinky = blinky; 
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public override Vector2 GetTarget(Ghost.States state)
        {
            if (state == States.Chase)
            {
                Vector2 pacmanOffset = pacman.Center + pacman.Direction *2;
                Vector2 blinkyVector = (pacmanOffset - blinky.Center) * 2;
                return this.Target = blinkyVector; 
            }

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
            drawHelper.DrawBox(Color.Cyan);
            drawHelper.Translate(-this.Position);
        }
    }
}
