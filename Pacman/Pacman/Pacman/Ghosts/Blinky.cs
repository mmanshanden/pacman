using Base;
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
                return this.Target = pacman.Center + Vector2.One * 0.5f;

            return base.GetTarget(state);
        }

        public override void Collision_GameObject(GameObject gameObject)
        {
            // we have collision!
        }


        public override void Draw(DrawHelper drawHelper)
        {
            if (this.State != States.Chase && 
                this.State != States.Scatter)
            {
                base.Draw(drawHelper);
                return;
            }

            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Red);
            drawHelper.Translate(-this.Position);
        }
    }
}
