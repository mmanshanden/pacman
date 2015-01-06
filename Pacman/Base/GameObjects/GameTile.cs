using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameTile : GameObject
    {
        public bool Collidable
        {
            get;
            set;
        }

        public GameTile Left
        {
            get;
            set;
        }
        public GameTile Right
        {
            get;
            set;
        }
        public GameTile Top
        {
            get;
            set;
        }
        public GameTile Bototm
        {
            get;
            set;
        }

        public GameTile()
        {
            this.Collidable = false;
        }
    }
}
