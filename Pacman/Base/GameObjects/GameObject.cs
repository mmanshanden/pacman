using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public class GameObject
    {
        #region Tree
        public GameObject Parent
        {
            get;
            set;
        }
        public GameObject Root
        {
            get
            {
                if (this.Parent != null)
                    return Parent.Root;

                return this;
            }
        }
        #endregion

        #region Position
        public Vector2 Position
        {
            get;
            set;
        }
        public Vector2 Center
        {
            get
            {
                return this.Position + Collision.HalfVector;
            }
            set
            {
                this.Position = value - Collision.HalfVector;
            }
        }
        public Point Tile
        {
            get
            {
                return Collision.ToPoint(this.Position);
            }
        }
        #endregion

        public GameObject()
        {

        }

        public List<GameObject> GetTree()
        {
            List<GameObject> tree = new List<GameObject>();
            GameObject highest = this;

            while (highest.Parent != null)
            {
                tree.Add(highest.Parent);
                highest = highest.Parent;
            }

            return tree;
        }

        public virtual void Collision_GameObject(GameObject gameObject)
        {

        }

        public virtual bool CollidesWith(GameObject gameObject)
        {
            return (gameObject.Tile == this.Tile);
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw(DrawHelper drawHelper)
        {

        }
    }
}
