using _3dgl;
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

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.SetColor(Color.Orange);
            mb.PrimitiveBatch.DrawCube();

            Game.DrawManager.ModelLibrary.EndModel("clyde");
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

            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("clyde");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
        }

        public static Clyde LoadClyde(FileReader file)
        {
            Clyde clyde = new Clyde();
            clyde.Spawn = file.ReadVector("clyde_position");
            clyde.Scatter = file.ReadVector("clyde_scatter");
            clyde.Direction = Vector2.UnitY * -1;

            return clyde;
        }
    }
}
