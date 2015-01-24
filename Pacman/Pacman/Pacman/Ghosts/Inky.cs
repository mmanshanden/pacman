using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Inky : Ghost
    {
        // Returns target of Inky depending on its state
        public override Vector2 GetTarget(Ghost.States state)
        {
            Pacman pacman = this.GhostHouse.GetPacman();
            Blinky blinky = this.GhostHouse.Blinky;

            if (blinky == null)
                return this.Scatter;

            if (state == States.Chase)
            {
                Vector2 pacmanOffset = pacman.Center + pacman.Direction * 2;
                Vector2 blinkyVector = pacmanOffset + (pacmanOffset - blinky.Center);
                return blinkyVector; 
            }

            return base.GetTarget(state);
        }

        // Load Inky's 3D Model
        public new static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1f, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/inky", 15);
            modelLibrary.EndModel("inky");
        }

        // Draw Inky's 3D model
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
            drawManager.DrawModel("inky");
            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Inky LoadInky(FileReader file, int index = 0)
        {
            string i = "";
            if (index != 0)
                i = index.ToString();

            Inky inky = new Inky();
            inky.Spawn = file.ReadVector("inky" + i + "_position");
            inky.Scatter = file.ReadVector("inky" + i + "_scatter");
            inky.Direction = Vector2.UnitY * -1;
            inky.waitTime = 8; 
            inky.waitTimer = 8; 

            return inky;
        }
    }
}
