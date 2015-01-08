using Microsoft.Xna.Framework;

namespace Base
{
    public class GameTile : GameObject
    {
        public bool Collidable
        {
            get;
            set;
        }

        public Vector2 Center
        {
            get;
            set;
        }

        public GameBoard Board
        {
            get { return this.Parent as GameBoard; }
        }

        #region Surrounding Tiles
        public GameTile Left
        {
            get
            {
                return Board.Get(this.Tile.X - 1, this.Tile.Y) as GameTile;
            }
            
        }
        public GameTile Right
        {
            get
            {
                return Board.Get(this.Tile.X + 1, this.Tile.Y) as GameTile;
            }
        }
        public GameTile Top
        {
            get
            {
                return Board.Get(this.Tile.X, this.Tile.Y - 1) as GameTile;
            }
        }
        public GameTile Bottom
        {
            get
            {
                return Board.Get(this.Tile.X, this.Tile.Y + 1) as GameTile;
            }
        }
        #endregion

        public GameTile()
        {
            this.Collidable = false;
        }

        public int GetSurroundingTilesCount()
        {
            int count = 0;
            if (!Left.Collidable)
                count++;
            if (!Right.Collidable)
                count++;
            if (!Top.Collidable)
                count++;
            if (!Bottom.Collidable)
                count++;

            return count;
        }
    }
}
