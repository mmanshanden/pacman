using Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class GhostHouse : GameWorld
    {
        public Vector2 Entry
        {
            get;
            set;
        }

        public Blinky Blinky
        {
            get;
            set;
        }


        public GhostHouse()
            : base()
        {

        }

        public void Add(Blinky blinky)
        {
            this.Blinky = blinky;
            base.Add(blinky);
        }


        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);
        }

    }
}
