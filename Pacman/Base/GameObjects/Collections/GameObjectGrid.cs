using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameObjectGrid : GameObject
    {
        protected GameObject[,] grid;

        public Vector2 Size
        {
            get;
            set;
        }

        public GameObjectGrid(int width, int height)
        {
            this.grid = new GameObject[width, height];
            this.Size = new Vector2(width, height);
        }

        public void Add(GameObject gameObject, int x, int y)
        {
            if (gameObject == null)
                return;

            this.grid[x, y] = gameObject;
            gameObject.Parent = this;
            gameObject.Position = new Vector2(x, y);
        }

        public GameObject Get(int x, int y)
        {
            if (x < 0 || x >= this.Size.X)
                return null;

            if (y < 0 || y >= this.Size.Y)
                return null;

            return this.grid[x, y];
        }
        public GameObject Get(Point point)
        {
            return this.grid[point.X, point.Y];
        }

        public override void Update(float dt)
        {
            for (int x = 0; x < this.Size.X; x++)
            {
                for (int y = 0; y < this.Size.Y; y++)
                {
                    this.grid[x, y].Update(dt);
                }
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            foreach (GameObject gameObject in this.grid)
                gameObject.Draw(drawHelper);
        }

    }
}
