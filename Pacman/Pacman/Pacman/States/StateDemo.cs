using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        Level gameWorld;
        Pacman gameObject;
        Ghost ghost;

        public StateDemo()
        {
            this.gameWorld = new Level();
            this.gameObject = new Pacman();
            this.gameObject.Position = Vector2.One;

            this.gameWorld.Add(gameObject);
            this.gameObject.Speed = 4;
            this.gameObject.Direction = Vector2.UnitX;

            this.ghost = new Ghost();
            this.ghost.Position = Vector2.One;
            this.ghost.Direction = Vector2.UnitX;
            this.ghost.Speed = 4;
            this.gameWorld.Add(ghost);

            this.gameWorld.LoadLevel("Content/level1.txt");
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
            ghost.Target = gameObject.Position;
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
