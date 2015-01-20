using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : IGameState
    {
        GameServer server;

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby()
        {
            this.server = new GameServer();
            this.server.Start();

            Console.Clear();
            Console.WriteLine("Hosting lobby");
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
            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            LobbyMessage content = new LobbyMessage();
            content.PlayerCount = 1;
            content.GameMode = Network.GameModes.Multi;

            send.SetData(content);
            this.server.SetData(send);
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
        }

    }
}
