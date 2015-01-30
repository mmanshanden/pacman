﻿using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateJoinLobby : Menu
    {
        GameClient client;
        IGameState nextState;
        StateJoinGame game;

        LobbyPlayer self;
        IndexedGameObjectList others;
        private bool ready;

        public StateJoinLobby(string endpoint)
        {
            base.controlSprite = "back";

            // Connect to given serveraddress
            this.client = new GameClient();
            this.client.ConnectToServer(endpoint);

            this.self = new LobbyPlayer();
            this.others = new IndexedGameObjectList();
            this.ready = false;
        }
        public StateJoinLobby(GameClient client)
        {
            base.controlSprite = "back";

            this.client = client;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            // Go back to serverlist
            if (inputHelper.KeyDown(Keys.Back))
            {
                this.client.Disconnect();
                this.nextState = new MenuServerBrowser(); 
            }
        }

        public override IGameState TransitionTo()
        {
            if (this.game != null)
                return this.game;

            if (this.nextState != null)
                return this.nextState;

            if (!this.client.Connected)
                return new MenuErrorMessage("Could not connect to server.");

            return this;
        }

        public override void Update(float dt)
        {
            this.client.Update(dt);

            if (this.ready)
            {
                NetMessage send = new NetMessage();
                send.Type = PacketType.Lobby;

                NetMessageContent basemsg = new NetMessageContent();
                basemsg.Id = this.client.ConnectionID;

                if (basemsg.Id == 0)
                    return;

                send.SetData(this.self.UpdateMessage(basemsg));
                this.client.SetData(send);
            }


            NetMessage received = this.client.GetData();

            if (received == null)
                return;

            this.ready = true;
            
            if (received.Type == PacketType.WorldState)
            {
                this.game = new StateJoinGame(client, received);
                return;
            }

            if (received.Type != PacketType.Lobby)
                return;

            NetMessageContent cmsg;
            while ((cmsg = received.GetData()) != null)
            {
                LobbyMessage lmsg = (LobbyMessage)cmsg;

                if (!this.others.Contains(lmsg.Id))
                    this.others.Add(lmsg.Id, new LobbyPlayer());

                this.others.UpdateObject(lmsg);                
            }

        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.ready)
                this.self.Draw(drawHelper);
            else
            {
                drawHelper.DrawString("Connecting to server...", Vector2.One * 0.5f, DrawHelper.Origin.Center, Color.White);
            }
            base.Draw(drawHelper);
        }

    }
}
