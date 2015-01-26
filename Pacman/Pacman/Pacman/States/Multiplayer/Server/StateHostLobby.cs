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

        LobbyPlayer self;
        IndexedGameObjectList players;

        IGameState nextState;

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
                this.nextState = new MenuErrorMessage("Could not start server.");
                Console.WriteLine(e.ToString());
            }

            this.self = new LobbyPlayer();
            this.self.Position = new Vector2(0.1f, 0.1f);
            this.players = new IndexedGameObjectList();
            this.players.Add(0, self);

            Console.WriteLine("Hosting lobby");
            
        }

        public StateHostLobby(int levelIndex, GameServer server)
        {
            base.controlSprite = "lobbyhost";

            this.levelIndex = levelIndex;
            this.server = server;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.X) && this.players.Count > 1)
                this.nextState = new StateHostGame(this.server, this.levelIndex);

            if (inputHelper.KeyDown(Keys.Back))
            {
                this.server.Stop();

                this.nextState = new MenuServerBrowser();
            }

            this.self.HandleInput(inputHelper);
        }

        public override IGameState TransitionTo()
        {
            if (nextState != null)
                return this.nextState;

            return this;
        }

        public override void Update(float dt)
        {
            if (this.players.Count < 2 && this.levelIndex == 1)
                this.server.Visible = true;

            else
                this.server.Visible = false;


            NetMessage send = new NetMessage();
            send.Type = PacketType.Lobby;

            NetMessageContent basemsg = new NetMessageContent();
            this.players.WriteAllToMessage(send, basemsg);

            this.server.SetData(send);

            NetMessage received = this.server.GetData();

            if (received == null)
                return;

            int i = 2;
            NetMessageContent cmsg;
            while((cmsg = received.GetData()) != null)
            {
                if (cmsg.Id == 0)
                    return;

                LobbyMessage lmsg = (LobbyMessage)cmsg;

                if (!this.players.Contains(lmsg.Id))
                    this.players.Add(lmsg.Id, new LobbyPlayer());

                this.players.UpdateObject(lmsg.Id, lmsg);
                this.players.Get(lmsg.Id).Position = new Vector2(0.1f, 0.2f * i);

                i++;
            }

        }

        public override void Draw(DrawManager drawManager)
        {
            Game.Camera.SwitchToOrtho();

            this.players.Draw(drawManager);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            this.players.Draw(drawHelper);

            base.Draw(drawHelper);
        }
    }
}
