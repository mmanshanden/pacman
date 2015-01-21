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
        public override void Draw(DrawManager drawManager)
        {
            if (Left is GhostHouseVoid)
            {
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);                 
            }
            
            else if (Right is GhostHouseVoid)
            {
                drawManager.Translate(this.Position);
                drawManager.Translate(0.5f, 0);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-0.5f, 0);
                drawManager.Translate(-this.Position);
            }

            else if (Top is GhostHouseVoid)
            {
                drawManager.Rotate(MathHelper.PiOver2);
                drawManager.Translate(0, 0.5f);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Translate(0, -0.5f);
                drawManager.Rotate(-MathHelper.PiOver2);                
            }

            else if (Bottom is GhostHouseVoid)
            {
                drawManager.Rotate(MathHelper.PiOver2);
                drawManager.Translate(0, 1);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Translate(0, -1);
                drawManager.Rotate(-MathHelper.PiOver2);             
            }

            else if (Right.Bottom is GhostHouseVoid)
            {
                drawManager.Scale(1, 0.5f);
                drawManager.Translate(0.5f, 0.5f);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Translate(-0.5f, -0.5f);
                drawManager.Scale(1, 2);
            }
            else if (Right.Top is GhostHouseVoid)
            {
                drawManager.Scale(1, 0.5f);
                drawManager.Translate(0.5f, 0);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Translate(-0.5f, 0);
                drawManager.Scale(1, 2);
            }
            else if (Left.Top is GhostHouseVoid)
            {
                drawManager.Scale(1, 0.5f);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Scale(1, 2);
            }
            else if (Left.Bottom is GhostHouseVoid)
            {
                drawManager.Scale(1, 0.5f);
                drawManager.Translate(0, 0.5f);
                drawManager.Translate(this.Position);
                drawManager.DrawModel("boundary");
                drawManager.Translate(-this.Position);
                drawManager.Translate(0, -0.5f);
                drawManager.Scale(1, 2);
            }
            
        }
    }
}
