using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pinky : Ghost
    {
        public override Vector2 GetTarget(Ghost.States state)
        {
            Pacman pacman = this.GhostHouse.GetPacman();

            if (state == States.Chase)
                return pacman.Center + pacman.Direction * 4; 

            return base.GetTarget(state);
        }

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.8f);

            mb.BuildFromTexture("voxels/pinky", 16);
            Game.DrawManager.ModelLibrary.EndModel("pinky");
        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.State != States.Chase && this.State != States.Scatter)
            {
                base.Draw(drawHelper);
                return; 
            }

            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Pink);
            drawHelper.Translate(-this.Position);


            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("pinky");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
        }

        public static Pinky LoadPinky(FileReader file)
        {
            Pinky pinky = new Pinky();
            pinky.Spawn = file.ReadVector("pinky_position");
            pinky.Scatter = file.ReadVector("pinky_scatter");
            pinky.Direction = Vector2.UnitY * -1;

            return pinky;
        }
    }
}
