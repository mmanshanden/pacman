using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        GameWorld gameWorld;
        GameObject gameObject;

        public StateDemo()
        {
            this.gameWorld = new GameWorld();
            this.gameObject = new GameObject();

            this.gameWorld.Add(gameObject);
            this.gameObject.Speed = 0.5f;
            this.gameObject.Direction = Vector2.UnitX;
        }

        public void HandleInput(InputHelper inputHelper)
        {

        }

        public IGameState TransitionTo()
        {
            return this;
        }

        public void Update(float dt)
        {
            this.gameWorld.Update(dt);
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(20, 20);
            this.gameWorld.Draw(drawHelper);
            drawHelper.Scale(0.05f, 0.05f);
        }

    }
}
