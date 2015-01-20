using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class GhostHouseWall : Wall
    {
        public override void Draw(DrawHelper drawHelper)
        {
            if (Left is GhostHouseVoid)
            {
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);                 
            }
            
            else if (Right is GhostHouseVoid)
            {
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.Translate(0.5f, 0);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-0.5f, 0);
                Game.DrawManager.Translate(-this.Position);
            }

            else if (Top is GhostHouseVoid)
            {
                Game.DrawManager.Rotate(MathHelper.PiOver2);
                Game.DrawManager.Translate(0, 0.5f);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Translate(0, -0.5f);
                Game.DrawManager.Rotate(-MathHelper.PiOver2);                
            }

            else if (Bottom is GhostHouseVoid)
            {
                Game.DrawManager.Rotate(MathHelper.PiOver2);
                Game.DrawManager.Translate(0, 1);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Translate(0, -1);
                Game.DrawManager.Rotate(-MathHelper.PiOver2);             
            }

            else if (Right.Bottom is GhostHouseVoid)
            {
                Game.DrawManager.Scale(1, 0.5f);
                Game.DrawManager.Translate(0.5f, 0.5f);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Translate(-0.5f, -0.5f);
                Game.DrawManager.Scale(1, 2);
            }
            else if (Right.Top is GhostHouseVoid)
            {
                Game.DrawManager.Scale(1, 0.5f);
                Game.DrawManager.Translate(0.5f, 0);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Translate(-0.5f, 0);
                Game.DrawManager.Scale(1, 2);
            }
            else if (Left.Top is GhostHouseVoid)
            {
                Game.DrawManager.Scale(1, 0.5f);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Scale(1, 2);
            }
            else if (Left.Bottom is GhostHouseVoid)
            {
                Game.DrawManager.Scale(1, 0.5f);
                Game.DrawManager.Translate(0, 0.5f);
                Game.DrawManager.Translate(this.Position);
                Game.DrawManager.DrawModel("boundary");
                Game.DrawManager.Translate(-this.Position);
                Game.DrawManager.Translate(0, -0.5f);
                Game.DrawManager.Scale(1, 2);
            }
            
        }
    }
}
