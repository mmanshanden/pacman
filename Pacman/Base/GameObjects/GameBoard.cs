using Microsoft.Xna.Framework;

namespace Base
{
    public class GameBoard : GameObjectGrid
    {
        public GameBoard(int width, int height)
            : base(width, height)
        {

        }

        public bool IsInside(Point point)
        {
            return (
                point.X >= 0 ||
                point.Y >= 0 ||
                point.X < this.Size.X ||
                point.Y < this.Size.Y
            );
        }
        

        public GameTile Get(Point point)
        {
            return this.Get(point.X, point.Y) as GameTile;
        }
        public bool IsCollidable(Point point)
        {
            GameTile tile = this.Get(point);

            if (tile == null)
                return false;

            return tile.Collidable;
        }

        public override bool CollidesWith(GameObject gameObject)
        {
            return this.IsCollidable(gameObject.Tile);
        }
    }
}
