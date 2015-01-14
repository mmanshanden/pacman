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

            Console.Clear();
            Console.WriteLine("Joining server " + endpoint);
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

            NetMessage message = this.client.GetData();
            if (message == null)
                return;

            switch (message.Type)
            {
                case DataType.Playing:
                    PlayingMessage msg = message as PlayingMessage;
                    Console.WriteLine(msg.Players.Count + " players were sent");
                    Console.WriteLine(msg.Players[0].Position.ToString());
                    break;
            }

        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
            
        }

    }
}
