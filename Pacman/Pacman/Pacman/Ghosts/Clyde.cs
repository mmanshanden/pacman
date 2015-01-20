﻿using _3dgl;
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

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/clyde", 15);
            Game.DrawManager.ModelLibrary.EndModel("clyde");
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
            drawHelper.DrawBox(Color.Orange);
            drawHelper.Translate(-this.Position);

            float radians = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            Game.DrawManager.RotateOver(radians, Vector2.One * 0.5f);
            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("clyde");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
            Game.DrawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }

        public static Clyde LoadClyde(FileReader file)
        {
            Clyde clyde = new Clyde();
            clyde.Spawn = file.ReadVector("clyde_position");
            clyde.Scatter = file.ReadVector("clyde_scatter");
            clyde.Direction = Vector2.UnitY * -1;
            clyde.waitTime = 4; 
            clyde.waitTimer = 4; 

            return clyde;
        }
    }
}
