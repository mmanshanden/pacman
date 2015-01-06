using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameObjectGrid : GameObject
    {
        GameObject[,] grid;

        public Vector2 Size
        {
            get;
            set;
        }

        public GameObjectGrid(int width, int height)
        {
            this.Size = new Vector2(width, height);
        }

        public void Add(GameObject gameObject, int x, int y)
        {
            this.grid[x, y] = gameObject;
            gameObject.Parent = this;
            gameObject.Position = new Vector2(x, y);
        }

        public GameObject Get(int x, int y)
        {
            return this.grid[x, y];
        }

    }
}
