using Microsoft.Xna.Framework;
namespace Pacman
{
    class Blinky : Ghost
    {
        private Pacman pacman;

        public Blinky(Pacman pacman)
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
                return this.Target = pacman.Position + Vector2.One * 0.5f;

            return base.GetTarget(state);
        }


        public override void Draw(Base.DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            if (this.State == States.Chase)
            {
                drawHelper.DrawBox(Color.Red);
            }
            else if (this.State == States.Frightened)
            {
                drawHelper.DrawBox(Color.Purple);
            }

            else if (this.State == States.Scatter)
            {
                drawHelper.DrawBox(Color.Cyan);
            }
            else if (this.State == States.Dead)
            {
                drawHelper.DrawBox(Color.Pink);
            }
            drawHelper.Translate(-this.Position);
        }
    }
}
