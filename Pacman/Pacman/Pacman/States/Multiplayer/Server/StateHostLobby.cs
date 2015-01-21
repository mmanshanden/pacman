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
        StateHostGame game; 

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby()
        {
            base.controlSprite = "menu_controls_kb_lobbyhost";

            this.server = new GameServer();
            this.server.Start();

            this.lobbyState = new LobbyMessage();

            Console.Clear();
            Console.WriteLine("Hosting lobby");
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.Y) && this.lobbyState.PlayerCount > 1)
                this.game = new StateHostGame(this.server);
            if (inputHelper.KeyDown(Keys.Back))
            {
                // Handle server stop properly (server.Stop() causes crash on re-host) ! ##
            }
        }

        public override IGameState TransitionTo()
        {
            if (game != null)
                return this.game;

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
