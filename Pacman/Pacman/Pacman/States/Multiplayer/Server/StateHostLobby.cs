using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : IGameState
    {
        GameServer server;
        IGameState nextState; 

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
            if (inputHelper.KeyDown(Keys.Y))
                this.nextState = new StateHostGame(this.server); 
        }

        public IGameState TransitionTo()
        {
            if (nextState != null)
                return this.nextState;

            return this;
        }

        public void Update(float dt)
        {
            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            LobbyMessage content = new LobbyMessage();
            content.PlayerCount = 1 + this.server.GetConnections().Count;
            content.GameMode = Network.GameModes.Multi;

            send.SetData(content);
            this.server.SetData(send);
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;

            Console.Clear();

            foreach (string ip in this.server.GetConnections())
                Console.WriteLine(ip);

        }

    }
}
