using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    public class GameObject
    {
        protected Vector2 position;

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
        public Vector2 Position
        {
            get { return this.position; }
            set { this.position = value; }
        }
        public Point Tile
        {
            get
            {
                return Collision.ToPoint(this.Position);
            }
        }

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

        public Vector2 GetGlobalPosition()
        {
            if (this.Parent != null)
                return this.Parent.GetGlobalPosition() + this.Position;

            return this.Position;
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
