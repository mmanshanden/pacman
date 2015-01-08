using Microsoft.Xna.Framework;

namespace Base
{
    public class GameCharacter : GameObjectList
    {
        public Vector2 Direction
        {
            get;
            set;
        }
        public float Speed
        {
            get;
            set;
        }
        public Vector2 Velocity
        {
            get { return this.Direction * this.Speed; }
        }
        public GameWorld World
        {
            get { return this.Parent as GameWorld; }
        }

        public GameCharacter()
        {
            this.Direction = Vector2.Zero;
            this.Speed = 1;
        }

        public void Move(GameBoard board, float dt)
        {
            if (this.Velocity == Vector2.Zero)
                return;


            this.Position += this.Velocity * dt;
        }

    }
}
