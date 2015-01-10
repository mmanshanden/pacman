using Microsoft.Xna.Framework;
namespace Pacman
{
    class Blinky : Ghost
    {
        private Pacman pacman;

        public Blinky(Vector2 spawn, Pacman pacman)
            : base(spawn)
        {
            this.pacman = pacman;
        }

        public override void Update(float dt)
        {
            this.Target = pacman.Position + Vector2.One * 0.5f;
        }

        public override void Draw(Base.DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Red);
            drawHelper.Translate(-this.Position);
        }
    }
}
