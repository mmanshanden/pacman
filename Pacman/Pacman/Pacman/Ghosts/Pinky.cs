using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pinky : Ghost
    {
        // Returns target of Pinky depending on its state
        public override Vector2 GetTarget(Ghost.States state)
        {
            Pacman pacman = this.GhostHouse.GetPacman();

            if (state == States.Chase)
                return pacman.Center + pacman.Direction * 4; 

            return base.GetTarget(state);
        }

        //Load Pinky's 3D model
        public new static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/pinky", 15);
            modelLibrary.EndModel("pinky");
        }

        // Draw Pinky's 3D Model
        public override void Draw(DrawManager drawManager)
        {
            // If Dead or Frightened we draw a Blue/White ghost model
            if (this.State == States.Dead ||
                this.State == States.Frightened)
            {
                base.Draw(drawManager);
                return;
            }
            
            float radians = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            drawManager.RotateOver(radians, Vector2.One * 0.5f);
            drawManager.Translate(this.Position.X, this.Position.Y);
            drawManager.DrawModel("pinky");
            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Pinky LoadPinky(FileReader file, int index = 0)
        {
            string i = "";
            if (index != 0)
                i = index.ToString();

            Pinky pinky = new Pinky();
            pinky.Spawn = file.ReadVector("pinky" + i + "_position");
            pinky.Scatter = file.ReadVector("pinky" + i + "_scatter");
            pinky.Direction = Vector2.UnitY * -1;
            pinky.waitTime = 12;
            pinky.waitTimer = 12; 

            return pinky;
        }
    }
}
