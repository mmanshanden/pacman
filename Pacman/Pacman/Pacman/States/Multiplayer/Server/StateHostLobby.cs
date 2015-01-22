using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : Menu
    {
        GameServer server;
        LobbyMessage lobbyState;

        IGameState nextState;

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby()
        {
            base.controlSprite = "lobbyhost";

            this.server = new GameServer();
            this.server.Start();

            this.lobbyState = new LobbyMessage();

            Console.Clear();
            Console.WriteLine("Hosting lobby");
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.X))
                this.nextState = new StateHostGame(this.server);

            if (inputHelper.KeyDown(Keys.Back))
            {
                this.server.Stop();

                this.nextState = new MenuServerBrowser();
            }
        }

        public override IGameState TransitionTo()
        {
            if (nextState != null)
                return this.nextState;

            return this;
        }

        public override void Update(float dt)
        {
            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            lobbyState.PlayerCount = 1 + this.server.GetConnections().Count;
            lobbyState.GameMode = Network.GameModes.Multi;

            send.SetData(lobbyState);
            this.server.SetData(send);
        }
        
        public override void Draw(DrawHelper drawHelper)
        {
            //Console.Visible = true;

            Console.Clear();

            foreach (string ip in this.server.GetConnections())
                Console.WriteLine(ip);

            base.Draw(drawHelper);
        }
    }
}
