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
            if (state == States.Wait)
                    return this.GhostHouse.Center;
            return base.GetTarget(state);
        }
        
        public new static void Load(ModelLibrary modelLibrary)
        {
            // load blinky
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/blinky", 15);

            modelLibrary.EndModel("blinky");
        }

        public override void Draw(DrawManager drawManager)
        {
            if (this.State == States.Dead || 
                this.State == States.Frightened)
            {
                base.Draw(drawManager);
                return;
            }

            float radians = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            drawManager.RotateOver(radians, Vector2.One * 0.5f);
            drawManager.Translate(this.Position.X, this.Position.Y);
            drawManager.DrawModel("blinky");
            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Blinky LoadBlinky(FileReader file, int index = 0)
        {
            string i = "";
            if (index != 0)
                i = index.ToString();

            Blinky blinky = new Blinky();
            blinky.Spawn = file.ReadVector("blinky" + i + "_position");
            blinky.Scatter = file.ReadVector("blinky" + i + "_scatter");
            blinky.Direction = Vector2.UnitY * -1;
            blinky.waitTime = 0; 
            blinky.waitTimer = 0; 

            return blinky;
        }
    }
}
