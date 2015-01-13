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
            this.Add(blinky as GameObject);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            this.Blinky.Draw(drawHelper);


        }

        
    }
}
