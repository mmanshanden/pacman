using Microsoft.Xna.Framework;

namespace Base
{
    public class GameObject
    {
        public GameObject Parent
        {
            get;
            set;
        }
        public Vector2 Position
        {
            get;
            set;
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

        public virtual void Collision_GameObject(GameObject gameObject)
        {

        }

        public virtual bool CollidesWith(GameObject gameObject)
        {
            return false;
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw(DrawHelper drawHelper)
        {

        }
    }
}
