using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public class GameTile : GameObject
    {
        public GameBoard Board
        {
            get { return this.Parent as GameBoard; }
        }

        #region Surrounding Tiles
        public GameTile Left
        {
            get { return Board.GetTile(this.Center + Collision.Left); }
        }
        public GameTile Right
        {
            get { return Board.GetTile(this.Center + Collision.Right); }
        }
        public GameTile Top
        {
            get { return Board.GetTile(this.Center + Collision.Up); }
        }
        public GameTile Bottom
        {
            get { return Board.GetTile(this.Center + Collision.Down); }
        }
        #endregion

        public virtual bool IsCollidable(GameObject gameObject)
        {
            return false;
        }

        public List<GameTile> GetNeighbourList(GameObject gameObject)
        {
            List<GameTile> result = new List<GameTile>();

            if (this.Left != null && !this.Left.IsCollidable(gameObject))
                result.Add(this.Left);

            if (this.Right != null && !this.Right.IsCollidable(gameObject))
                result.Add(this.Right);

            if (this.Top != null && !this.Top.IsCollidable(gameObject))
                result.Add(this.Top);

            if (this.Bottom != null && !this.Bottom.IsCollidable(gameObject))
                result.Add(this.Bottom);

            return result;
        }

        public int  GetNeighbourCount(GameObject gameObject)
        {
            return this.GetNeighbourList(gameObject).Count;
        }

        public virtual void Load(_3dgl.ModelBuilder model)
        {

        }

    }
}
