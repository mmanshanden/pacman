using _3dgl;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    /// <summary>
    /// Adds objects to a grid of given size and width.
    /// Added objects will get position set to position
    /// in grid. Draw and Update methods will be called
    /// for all objects added to the list.
    /// </summary>
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
            foreach (GameObject gameObject in this.grid)
            {
                if (gameObject != null)
                    gameObject.Update(dt);
            }
        }

        public override void Draw(DrawManager drawManager)
        {
            foreach (GameObject gameObject in this.grid)
            {
                if (gameObject != null)
                    gameObject.Draw(drawManager);
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            foreach (GameObject gameObject in this.grid)
            {
                if (gameObject != null)
                    gameObject.Draw(drawHelper);
            }
        }
    }
}
