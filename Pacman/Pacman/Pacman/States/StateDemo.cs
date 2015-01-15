using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        Level gameWorld;

        public StateDemo()
        {
            this.gameWorld = new Level("Content/level1.txt");
            this.gameWorld.LoadGameBoard("level");

            Player player = new Player();
            this.gameWorld.Add(player, "player_position");
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.gameWorld.HandleInput(inputHelper);
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
