using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Clyde : Ghost
    {
        public override Vector2 GetTarget(Ghost.States state)
        {
            Pacman pacman = this.GhostHouse.GetPacman();

            if (state == States.Chase)
            {
                if (Vector2.Distance(this.Center, pacman.Center) > 5)
                    return pacman.Center;
                else
                    return this.Scatter;
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

        public static Clyde LoadClyde(FileReader file)
        {
            Clyde clyde = new Clyde();
            clyde.Spawn = file.ReadVector("clyde_position");
            clyde.Scatter = file.ReadVector("clyde_scatter");
            clyde.Direction = Vector2.UnitY;

            return clyde;
        }
    }
}
