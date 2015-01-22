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
            this.server.StartSimple();

            this.lobbyState = new LobbyMessage();

            Console.Clear();
            Console.WriteLine("Hosting lobby");
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.X) && this.lobbyState.PlayerCount > 1)
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
            if (this.levelIndex == 1)
                this.server.Visible = true;

            this.server.Update(dt);

            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            lobbyState.PlayerCount = 1 + this.server.GetConnectedIPs().Count;
            lobbyState.GameMode = Network.GameModes.Multi;

            send.SetData(lobbyState);
            this.server.SetData(send);
        }
        
        public override void Draw(DrawHelper drawHelper)
        {
            //Console.Visible = true;

            Console.Clear();

            foreach (string ip in this.server.GetConnectedIPs())
                Console.WriteLine(ip);

            base.Draw(drawHelper);
        }
    }
}
