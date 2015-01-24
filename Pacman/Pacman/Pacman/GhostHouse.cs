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
        public float FrightenedDuration
        {
            get;
            set;
        }

        private Pacman pacman;
        private List<Ghost> ghosts; 

        protected Level Level
        {
            get
            {
                return this.Parent as Level;
            }
        }

        public Vector2 Entry { get; set; }
        
        public Blinky Blinky { get; private set; }
        public Inky Inky { get; private set; }
        public Pinky Pinky { get; private set; }
        public Clyde Clyde { get; private set; }

        public GhostHouse()
        {
            this.ghosts = new List<Ghost>();
            this.FrightenedDuration = 6.9f;
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
            this.ghosts.Add(ghost);
            this.Level.Add(ghost);
            ghost.GhostHouse = this;
        }
        #endregion

        public void SetPacman(Pacman pacman)
        {
            this.pacman = pacman;
        }

        public Pacman GetPacman()
        {
            return this.pacman;
        }

        // Set all ghosts to begin state
        public void ResetGhosts()
        {
            foreach (Ghost ghost in this.ghosts)
                ghost.Respawn(); 
        }

        // All ghosts that are outside the ghosthouse
        // Will be frightened unless they are dead
        public void FrightenGhosts()
        {
            foreach (Ghost ghost in this.ghosts)
                ghost.Frighten();
        }

        public override void Update(float dt)
        {
            
        }
    }
}
