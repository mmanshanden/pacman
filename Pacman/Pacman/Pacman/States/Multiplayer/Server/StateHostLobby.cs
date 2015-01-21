using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : IGameState
    {
        GameServer server;
        LobbyMessage lobbyState;

        StateHostGame game; 

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby()
        {
            this.server = new GameServer();
            this.server.Start();

            this.lobbyState = new LobbyMessage();

            Console.Clear();
            Console.WriteLine("Hosting lobby");
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.Y) && this.lobbyState.PlayerCount > 1)
                this.game = new StateHostGame(this.server); 
        }

        public IGameState TransitionTo()
        {
            if (game != null)
                return this.game;

            return this;
        }

        public void Update(float dt)
        {
            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            lobbyState.PlayerCount = 1 + this.server.GetConnections().Count;
            lobbyState.GameMode = Network.GameModes.Multi;

            send.SetData(lobbyState);
            this.server.SetData(send);
        }

        public void Draw(DrawManager drawManager)
        {
            Console.Visible = true;

            Console.Clear();

            foreach (string ip in this.server.GetConnections())
                Console.WriteLine(ip);

        }

    }
}
