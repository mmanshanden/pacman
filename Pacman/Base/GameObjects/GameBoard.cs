using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    /// <summary>
    /// GameObjectGrid intended for GameTiles
    /// </summary>
    public class GameBoard : GameObjectGrid
    {
        public GameBoard(int width, int height)
            : base(width, height)
        {

        }
        
        /// <summary>
        /// Checks whether given position is inside the gameboard.
        /// </summary>
        public bool IsInside(Vector2 position)
        {
            return (
                position.X >= 0 && position.X < this.Size.X &&
                position.Y >= 0 && position.Y < this.Size.Y
            );
        }

        // see above
        public bool IsInside(Point point)
        {
            return (
                point.X >= 0 && point.X < this.Size.X &&
                point.Y >= 0 && point.Y < this.Size.Y
            );
        }

        /// <summary>
        /// Returns the gametile on given position.
        /// Return null if there is no gametile on
        /// given position.
        /// </summary>
        public GameTile GetTile(Vector2 position)
        {
            if (!IsInside(position))
                return null;

            Point point = Collision.ToPoint(position);
            return this.Get(point) as GameTile;
        }

        /// <summary>
        /// Returns a list of gametiles neighboring the tile in given position.
        /// Neighboring tiles are positioned directly above, below, left or right
        /// side of a given tile.
        /// Only collidable tiles are returned. Collision is tested against given
        /// gameobject.
        /// </summary>
        public List<GameTile> GetNeighbourList(Vector2 position, GameObject gameObject)
        {
            return this.GetTile(position).GetNeighbourList(gameObject);
        }

        /// <summary>
        /// Returns the count of gametiles neighboring the tile in given position.
        /// Neighboring tiles are positioned directly above, below, left or right
        /// side of a given tile.
        /// Only collidable tiles are counted. Collision is tested against given
        /// gameobject.
        /// </summary>
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

    }
}
