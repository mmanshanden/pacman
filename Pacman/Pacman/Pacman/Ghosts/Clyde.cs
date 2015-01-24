using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Clyde : Ghost
    {
        // Returns target of Clyde depending on its state
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

        // Load Clyde's 3D Model
        public new static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/clyde", 15);
            modelLibrary.EndModel("clyde");
        }

        // Draw Clyde's 3D Model
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
            drawManager.DrawModel("clyde");
            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Clyde LoadClyde(FileReader file, int index = 0)
        {
            string i = "";
            if (index != 0)
                i = index.ToString();

            Clyde clyde = new Clyde();
            clyde.Spawn = file.ReadVector("clyde" + i + "_position");
            clyde.Scatter = file.ReadVector("clyde" + i + "_scatter");
            clyde.Direction = Vector2.UnitY * -1;
            clyde.waitTime = 4; 
            clyde.waitTimer = 4; 

            return clyde;
        }
    }
}
