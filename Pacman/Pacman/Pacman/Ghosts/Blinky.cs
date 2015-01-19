using _3dgl;
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
        
        public override void Load()
        {
            // load blinky
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/blinky", 15);

            Game.DrawManager.ModelLibrary.EndModel("blinky");
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


            float radians = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            Game.DrawManager.RotateOver(radians, Vector2.One * 0.5f);
            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("blinky");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
            Game.DrawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Blinky LoadBlinky(FileReader file)
        {
            Blinky blinky = new Blinky();
            blinky.Spawn = file.ReadVector("blinky_position");
            blinky.Scatter = file.ReadVector("blinky_scatter");
            blinky.Direction = Vector2.UnitY * -1;

            return blinky;
        }
    }
}
