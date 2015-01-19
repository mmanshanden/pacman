using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Inky : Ghost
    {
        public override Vector2 GetTarget(Ghost.States state)
        {
            Pacman pacman = this.GhostHouse.GetPacman();
            Blinky blinky = this.GhostHouse.Blinky;

            if (state == States.Chase)
            {
                Vector2 pacmanOffset = pacman.Center + pacman.Direction *2;
                Vector2 blinkyVector = (pacmanOffset - blinky.Center) * 2;
                return blinkyVector; 
            }

            return base.GetTarget(state);
        }

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);
            mb.BuildFromTexture("voxels/inky", 16);

            Game.DrawManager.ModelLibrary.EndModel("inky");
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


            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("inky");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
        }

        public static Inky LoadInky(FileReader file)
        {
            Inky inky = new Inky();
            inky.Spawn = file.ReadVector("inky_position");
            inky.Scatter = file.ReadVector("inky_scatter");
            inky.Direction = Vector2.UnitY * -1;

            return inky;
        }
    }
}
