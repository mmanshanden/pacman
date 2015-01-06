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
            return this.grid[x, y];
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
            for (int x = 0; x < this.Size.X; x++)
            {
                for (int y = 0; y < this.Size.Y; y++)
                {
                    if (this.grid[x, y] != null)
                        this.grid[x, y].Draw(drawHelper);
                }
            }
        }

    }
}
