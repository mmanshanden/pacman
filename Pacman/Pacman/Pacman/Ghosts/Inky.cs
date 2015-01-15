using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Inky : Ghost
    {
        private Pacman pacman;

        public Inky(Pacman pacman)
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


        public override void Draw(DrawHelper drawHelper)
        {
            if (this.State != States.Chase && this.State != States.Scatter)
                base.Draw(drawHelper);

            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Cyan);
            drawHelper.Translate(-this.Position);
        }
    }
}
