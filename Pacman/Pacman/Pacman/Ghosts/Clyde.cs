using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Clyde : Ghost
    {
        private Pacman pacman;

        public Clyde(Pacman pacman)
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
            {
                if (Vector2.Distance(this.Center, pacman.Center) > 5)
                    return this.Target = pacman.Center;
                else
                    
                    return this.Target = new Vector2(1, 30); // SET TO SCATTER TILE
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
            drawHelper.DrawBox(Color.Orange);
            drawHelper.Translate(-this.Position);
        }
    }
}
