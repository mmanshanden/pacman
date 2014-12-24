using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        Level gameWorld;
        GameObject gameObject;

        public StateDemo()
        {
            this.gameWorld = new Level();
            this.gameObject = new GameObject();
            this.gameObject.Position = Vector2.One;

            this.gameWorld.Add(gameObject);
            this.gameObject.Speed = 1f;
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
            drawHelper.Scale(13, 13);
            this.gameWorld.Draw(drawHelper);
            drawHelper.Scale(1/13f, 1/13f);
        }

    }
}
