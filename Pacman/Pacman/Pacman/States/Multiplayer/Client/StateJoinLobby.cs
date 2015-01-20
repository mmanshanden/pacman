using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateJoinLobby : IGameState
    {
        GameClient client;
        LobbyMessage lobbyState;

        public StateJoinLobby(string endpoint)
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
            NetMessage received = this.client.GetData();

            if (received == null)
                return;

            switch (received.Type)
            {
                case PacketType.Lobby:
                    this.lobbyState = received.GetData() as LobbyMessage;
                    break;
            }
        }

        public void Draw(DrawHelper drawHelper)
        {
            if (this.lobbyState != null)
            {
                Console.WriteLine("We have lobby!");
            }
        }

    }
}
