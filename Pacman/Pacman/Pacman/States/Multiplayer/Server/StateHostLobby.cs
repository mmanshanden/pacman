using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : Menu
    {
        int levelIndex;
        GameServer server;
        LobbyMessage lobbyState;

        IGameState nextState;

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby(int levelIndex)
        {
            base.controlSprite = "lobbyhost";

            this.levelIndex = levelIndex;
            this.server = new GameServer();
            try
            {
                this.server.Start();
            }
            catch (System.Net.Sockets.SocketException e)
            {
                this.nextState = new MenuErrorMessage("Could not start server. Socket unavailable.");
            }

            this.lobbyState = new LobbyMessage();

            Console.WriteLine("Hosting lobby");
        }

        public StateHostLobby(int levelIndex, GameServer server)
        {
            base.controlSprite = "lobbyhost";

            this.levelIndex = levelIndex;
            this.server = server;

            this.lobbyState = new LobbyMessage();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.X) && this.lobbyState.PlayerCount > 1)
                this.nextState = new StateHostGame(this.server, this.levelIndex);

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
            if (this.server.GetConnectedIPs().Count == 0 && this.levelIndex == 1)
                this.server.Visible = true;

            else
                this.server.Visible = false;
            
            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            lobbyState.PlayerCount = 1 + this.server.GetConnectedIPs().Count;

            send.SetData(lobbyState);
            this.server.SetData(send);
            this.server.ClearData();
        }
        
        public override void Draw(DrawHelper drawHelper)
        {
            int i = 0;

            foreach (string ip in this.server.GetConnectedIPs())
            {
                if (i > 6)
                    break;

                Vector2 position = Vector2.One * 0.1f;
                position.Y += 0.1f * i;

                drawHelper.DrawString(ip, position, DrawHelper.Origin.TopLeft, Color.White);
                i++;
            }

            base.Draw(drawHelper);
        }
    }
}
