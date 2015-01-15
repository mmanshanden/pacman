using Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class GhostHouse : GameObject
    {
        public Level Level
        {
            get
            {
                return this.Parent as Level;
            }
        }

        public Vector2 Entry
        {
            get;
            set;
        }

        public Blinky Blinky { get; set; }
        public Inky Inky { get; set; }
        public Pinky Pinky { get; set; }
        public Clyde Clyde { get; set; }

        public GhostHouse()
            : base()
        {
        }

        #region Ghost Adding
        public void Add(Blinky blinky)
        {
            this.Blinky = blinky;
            this.Add(blinky as Ghost);
        }

        public void Add(Clyde clyde)
        {
            this.Clyde = clyde;
            this.Add(clyde as Ghost);
        }
       
        public void Add(Inky inky)
        {
            this.Inky = inky;
            this.Add(inky as Ghost);
        }

        public void Add(Pinky pinky)
        {
            this.Pinky = pinky;
            this.Add(pinky as Ghost);
        }

        public void Add(Ghost ghost)
        {
            this.Level.Add(ghost);
            ghost.GhostHouse = this;
        }
        #endregion

        public override void Update(float dt)
        {
            // perhaps do something with ghosts here...
        }
    }
}
