using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;
using System.Collections.Generic;

namespace Pacman
{
    class StateHostLobby : Menu
    {
        int levelIndex;
        GameServer server;

        LobbyPlayer self;
        IndexedGameObjectList players;
        List<LobbyPlayer> lplayers;

        private bool keepScore = false;

        IGameState nextState;

        public StateHostLobby(int levelIndex)
        {
            if (levelIndex < 5)
                base.controlSprite = "lobbyhost";
            else
                base.controlSprite = "back";

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
            this.lplayers = new List<LobbyPlayer>();
            this.lplayers.Add(self);

            Console.WriteLine("Hosting lobby");            
        }

        public StateHostLobby(int levelIndex, GameServer server, NetMessage gamedata, bool keepScore = false)
        {
            if (levelIndex < 5)
                base.controlSprite = "lobbyhost";
            else
                base.controlSprite = "back";

            this.levelIndex = levelIndex;
            this.server = server;

            this.keepScore = keepScore;
            this.players = new IndexedGameObjectList();
            this.lplayers = new List<LobbyPlayer>();

            NetMessageContent cmsg;
            while ((cmsg = gamedata.GetData()) != null)
            {
                if (cmsg.Type != DataType.Pacman)
                    continue;

                PlayerMessage pmsg = (PlayerMessage)cmsg;

                if (pmsg.Id == 0)
                {
                    this.self = new LobbyPlayer();
                    this.self.Score = pmsg.Score;
                    this.self.Id = 0;
                    this.players.Add(0, self);
                    this.lplayers.Add(self);
                }
                else
                {
                    LobbyPlayer player = new LobbyPlayer();
                    player.Score = pmsg.Score;
                    player.Id = pmsg.Id;
                    this.players.Add(pmsg.Id, player);
                    this.lplayers.Add(player);
                }
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.X) && this.players.Count > 1 && this.levelIndex < 5)
            {
                if (this.keepScore == false)
                {
                    foreach (LobbyPlayer l in this.lplayers)
                        l.Score = 0;
                }

                this.nextState = new StateHostGame(this.server, this.lplayers, this.levelIndex);
                this.server.Visible = false;
            }
            
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
            if (this.players.Count < 3 && this.levelIndex == 1)
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


            if (received.Type == PacketType.Logout)
            {
                this.players.Remove(received.ConnectionId);
                return;
            }
            
            if (received.Type != PacketType.Lobby)
                return;


            int i = 2;
            NetMessageContent cmsg;
            while((cmsg = received.GetData()) != null)
            {
                if (cmsg.Id == 0)
                    return;

                LobbyMessage lmsg = (LobbyMessage)cmsg;

                if (!this.players.Contains(lmsg.Id))
                {   
                    LobbyPlayer pl = new LobbyPlayer();
                    pl.Id = lmsg.Id;
                    this.players.Add(lmsg.Id, pl);
                    this.lplayers.Add(pl);
                }

                this.players.UpdateObject(lmsg.Id, lmsg);
                i++;
            }

        }

        public override void Draw(DrawManager drawManager)
        {
            this.self.Position = new Vector2(0.25f, 0.25f);
            this.self.Draw(drawManager);

            if (this.server.GetConnections().Count < 1 || this.players.Count < 2)
                return;

            int partnerId = this.server.GetConnections()[0].Id;
            GameObject other = this.players.Get(partnerId);
            other.Position = new Vector2(0.75f, 0.25f);
            other.Draw(drawManager);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);

            this.self.Name = "You";
            this.self.Draw(drawHelper);

            if (this.server.GetConnections().Count < 1 || this.players.Count < 2)
                return;

            int partnerId = this.server.GetConnections()[0].Id;
            GameObject other = this.players.Get(partnerId);
            (other as LobbyPlayer).Name = "Partner";
            other.Position = new Vector2(0.75f, 0.25f);
            other.Draw(drawHelper);

            int totalscore = 0;

            foreach (LobbyPlayer lplayer in this.lplayers)
                totalscore += lplayer.Score;

            if (totalscore > 0)
                drawHelper.DrawStringBig("Total Score: " + totalscore, new Vector2(0.5f, 0.2f), DrawHelper.Origin.Center, Color.White);
        }
    }
}
