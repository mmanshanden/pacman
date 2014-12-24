using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        Level gameWorld;
        Pacman gameObject;

        public StateDemo()
        {
            this.gameWorld = new Level();
            this.gameObject = new Pacman();
            this.gameObject.Position = Vector2.One;

            this.gameWorld.Add(gameObject);
            this.gameObject.Speed = 4;
            this.gameObject.Direction = Vector2.UnitX;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            gameObject.Direction = inputHelper.GetDirectionalInput();
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
