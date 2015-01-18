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
        public enum AiModes
        {
            First,
            Random,
            Nearby,
        }

        private List<Pacman> pacmans;
        private List<Ghost> ghosts; 

        protected Level Level
        {
            get
            {
                return this.Parent as Level;
            }
        }

        public AiModes AiMode { get; set; }

        public Vector2 Entry { get; set; }
        
        public Blinky Blinky { get; private set; }
        public Inky Inky { get; private set; }
        public Pinky Pinky { get; private set; }
        public Clyde Clyde { get; private set; }

        public GhostHouse()
        {
            this.pacmans = new List<Pacman>();
            this.ghosts = new List<Ghost>();
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

        public void AddPacman(Pacman pacman)
        {
            this.pacmans.Add(pacman);
        }

        public Pacman GetPacman()
        {
            switch (this.AiMode)
            {
                case AiModes.Random:
                    int index = Game.Random.Next(this.pacmans.Count);
                    return this.pacmans[index];

                default:
                    this.AiMode = AiModes.Random;
                    return GetPacman();
            }
        }

        public void ResetGhosts()
        {
            foreach (Ghost ghost in ghosts)
                ghost.Respawn(); 
        }

        public override void Update(float dt)
        {
            // perhaps do something with ghosts here...
        }
    }
}
