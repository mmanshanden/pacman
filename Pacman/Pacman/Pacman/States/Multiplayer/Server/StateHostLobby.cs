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
            NetMessage msg = new NetMessage();
            msg.Type = PacketType.Lobby;

            LobbyMessage lobbyMsg = new LobbyMessage();
            lobbyMsg.PlayerCount = 1;
            lobbyMsg.GameMode = Network.GameModes.Multi;

            msg.SetData(lobbyMsg);
            this.server.SetData(msg);
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
        }

    }
}
