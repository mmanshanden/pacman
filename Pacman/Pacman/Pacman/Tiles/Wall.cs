using Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class Wall : GameTile
    {
        public Wall()
            : base()
        {
            this.Collidable = true;
        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Blue);
            drawHelper.Translate(-this.Position);
        }
    }
}
