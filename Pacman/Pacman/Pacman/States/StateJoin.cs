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

            NetMessage baremsg = this.client.GetData();

            if (baremsg == null)
                return;

            switch (baremsg.DataType)
            {
                case DataType.Playing:
                    PlayingMessage playermsg = new PlayingMessage();
                    NetMessage.CopyOver(baremsg, playermsg);
                    Console.WriteLine(playermsg.ToString());
                    break;
            }

        }

        public void Draw(DrawHelper drawHelper)
        {
            
        }

    }
}
