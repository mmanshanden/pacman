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
            this.Target = pacman.Position + Vector2.One * 0.5f;
            base.Update(dt);
        }

        public override void Draw(Base.DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Red);
            drawHelper.Translate(-this.Position);
        }
    }
}
