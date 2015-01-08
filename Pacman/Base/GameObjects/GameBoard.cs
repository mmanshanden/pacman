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

        public void Add(GameTile tile, int x, int y)
        {
            tile.Center = new Vector2(x + 0.5f, y + 0.5f);
            this.Add(tile as GameObject, x, y);
        }

        public GameTile Get(Vector2 position)
        {
            Vector2 r = position - this.Position;
            Point tile = Collision.ToPoint(r);
            return this.Get(tile) as GameTile;
        }

        public bool IsCollidable(Vector2 position)
        {
            GameTile tile = this.Get(position);

            if (tile == null)
                return false;

            return tile.Collidable;
        }

        public override bool CollidesWith(GameObject gameObject)
        {
            return this.IsCollidable(gameObject.Position);
        }
    }
}
