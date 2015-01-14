using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateJoin : IGameState
    {
        GameClient client;

        public StateJoin(string endpoint)
        {
            this.client = new GameClient();
            this.client.ConnectToServer(endpoint);
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
            this.client.Update(dt);
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
            Console.Clear();
            Console.WriteLine("In StateJoin");
            
        }

    }
}
