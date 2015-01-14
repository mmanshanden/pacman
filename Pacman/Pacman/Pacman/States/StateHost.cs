using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateHost : IGameState
    {
        GameServer server;

        public StateHost()
        {
            this.server = new GameServer();
            this.server.StartSimple();

            Console.Clear();
            Console.WriteLine("Hosting server");
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
            this.server.Update(dt);
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
        }

    }
}
