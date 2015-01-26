using _3dgl;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public partial class GameObject
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
        private Vector2 position;
        public Vector2 Position
        {
            get { return this.position; }
            set
            {
                if (value.X == 33 && value.Y == 11 ||
                    value.X == 11 && value.Y == 33)
                {
                    Console.WriteLine("ZOMG");
                }

                this.position = value;
            }
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

        /// <summary>
        /// Returns the tree of parent objects.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Is called when gameobject collides with another gameobject.
        /// </summary>
        public virtual void Collision_GameObject(GameObject gameObject)
        {

        }

        /// <summary>
        /// Checks whether this gameobject collides with given gameobject.
        /// </summary>
        public virtual bool CollidesWith(GameObject gameObject)
        {
            // AABB
            return (
              this.Position.X < gameObject.Position.X + 1 &&
              this.Position.X + 1 > gameObject.Position.X &&
              this.Position.Y < gameObject.Position.Y + 1 &&
              this.Position.Y + 1 > gameObject.Position.Y
            );
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw(DrawManager drawManager)
        {

        }

        public virtual void Draw(DrawHelper drawHelper)
        {

        }
    }
}
