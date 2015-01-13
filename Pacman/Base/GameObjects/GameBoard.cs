using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public class GameBoard : GameObjectGrid
    {
        public GameBoard(int width, int height)
            : base(width, height)
        {

        }

        public bool IsInside(Vector2 position)
        {
            Point point = Collision.ToPoint(position);
            return this.IsInside(point);
        }
        public bool IsInside(Point point)
        {
            return (
                point.X >= 0 && point.X < this.Size.X &&
                point.Y >= 0 && point.Y < this.Size.Y
            );
        }

        public GameTile GetTile(Vector2 position)
        {
            if (!IsInside(position))
                return null;

            Point point = Collision.ToPoint(position);
            return this.Get(point) as GameTile;
        }
        public List<GameTile> GetNeighbourList(Vector2 position, GameObject gameObject)
        {
            return this.GetTile(position).GetNeighbourList(gameObject);
        }
        public int GetNeighbourCount(Vector2 position, GameObject gameObject)
        {
            return this.GetTile(position).GetNeighbourCount(gameObject);
        }

        #region GameObject overloads
        public bool IsInside(GameObject gameObject)
        {
            return this.IsInside(gameObject.Center);
        }
        public GameTile GetTile(GameObject gameObject)
        {
            return this.GetTile(gameObject.Center);
        }
        public List<GameTile> GetNeighbourList(GameObject gameObject)
        {
            return this.GetNeighbourList(gameObject.Center, gameObject);
        }
        public int GetNeighbourCount(GameObject gameObject)
        {
            return this.GetNeighbourCount(gameObject.Center, gameObject);
        }
        #endregion

        public static GameBoard CopyDimensions(char[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            return new GameBoard(width, height);
        }
    }
}
