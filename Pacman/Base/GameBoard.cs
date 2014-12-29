using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public class GameBoard
    {
        private short[,] grid;
        private short[] collidables;

        public Vector2 Size
        {
            get
            {
                Vector2 size = new Vector2();
                size.X = this.grid.GetLength(0);
                size.Y = this.grid.GetLength(1);

                return size;
            }
        }

        public GameBoard()
        {
            
        }
        public void SetGrid(short[,] grid, short[] collidables)
        {
            this.grid = grid;
            this.collidables = collidables;
        }
        public bool IsOutSide(Point tile)
        {
            return
            (
                tile.X < 0 ||
                tile.Y < 0 ||
                tile.X >= this.Size.X ||
                tile.Y >= this.Size.Y
            );
        }
        public bool IsCollidable(Point tile)
        {
            if (this.IsOutSide(tile))
                return true;

            foreach (short value in this.collidables)
                if (this.grid[tile.X, tile.Y] == value)
                    return true;

            return false;
        }
        public short GetTileValue(Point tile)
        {
            return this.grid[tile.X, tile.Y];
        }
        public int GetNeighbourCount(Point tile)
        {
            return this.GetNeighbourTiles(tile).Count;
        }
        public List<Point> GetNeighbourTiles(Point tile)
        {
            List<Point> result = new List<Point>();

            Point top = new Point(tile.X, tile.Y - 1);
            Point bottom = new Point(tile.X, tile.Y + 1);
            Point left = new Point(tile.X - 1, tile.Y);
            Point right = new Point(tile.X + 1, tile.Y);

            if (!this.IsCollidable(top))
                result.Add(top);

            if (!this.IsCollidable(bottom))
                result.Add(bottom);

            if (!this.IsCollidable(left))
                result.Add(left);

            if (!this.IsCollidable(right))
                result.Add(right);

            return result;
        }
        
        #region GameObject Overloads
        public bool IsOutSide(GameObject gameObject)
        {
            return this.IsOutSide(gameObject.Tile);
        }
        public bool IsCollidable(GameObject gameObject)
        {
            return this.IsCollidable(gameObject.Tile);
        }
        public short GetTileValue(GameObject gameObject)
        {
            return this.GetTileValue(gameObject.Tile);
        }
        public int GetNeighbourCount(GameObject gameObject)
        {
            return this.GetNeighbourCount(gameObject.Tile);
        }
        public List<Point> GetNeighbourTiles(GameObject gameObject)
        {
            return this.GetNeighbourTiles(gameObject.Tile);
        }
        #endregion

        public static GameBoard FactoryMethod()
        {
            return new GameBoard();
        }
    }
}
