using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Blinky : Ghost
    {
        public override Vector2 GetTarget(Ghost.States state)
        {
            if (state == States.Chase)
                return this.GhostHouse.GetPacman().Center;

            return base.GetTarget(state);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.State == States.Dead || 
                this.State == States.Frightened)
            {
                base.Draw(drawHelper);
                return;
            }

            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Red);
            drawHelper.Translate(-this.Position);
        }

        public static Blinky LoadBlinky(FileReader file)
        {
            Blinky blinky = new Blinky();
            blinky.Spawn = file.ReadVector("blinky_position");
            blinky.Scatter = file.ReadVector("blinky_scatter");
            blinky.Direction = Vector2.UnitY * -1;
            blinky.waitTime = 0; 
            blinky.waitTimer = 0; 

            return blinky;
        }
    }
}
