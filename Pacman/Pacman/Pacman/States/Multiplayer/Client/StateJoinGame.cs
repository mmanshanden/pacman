using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using _3dgl;

namespace Pacman
{
    class StateJoinGame : IGameState
    {
        const float MapUpdateInterval = 6;

        GameClient client;
        Level level;

        IGameState nextState; 

        IndexedGameObjectList players;
        OrderedGameObjectList ghosts;

        GameObjectList bubbles;
        GameObjectList powerups;

        private float mapUpdateTimer;
        private bool returnToLobby;

        public StateJoinGame(GameClient client, NetMessage gamemessage)
        {
            this.client = client;
                        
            this.level = new Level();
            this.players = new IndexedGameObjectList();
            this.ghosts = new OrderedGameObjectList();

            NetMessageContent cmsg;
            int updatecount = 0;
            while((cmsg = gamemessage.GetData()) != null)
            {
                switch(cmsg.Type)
                {
                    // set spawn position for self
                    case DataType.Pacman:
                        PlayerMessage pmsg = (PlayerMessage)cmsg;

                        if (cmsg.Id == this.client.ConnectionID)
                        {
                            Player player = new Player();
                            player.Spawn = pmsg.Position;
                            this.level.Add(player);
                        }
                        else
                        {
                            Pacman player = new Pacman();
                            player.Spawn = pmsg.Position;

                            this.level.Add(player);
                            this.players.Add(pmsg.Id, player);
                        }
                        break;

                    // load map and map object
                    case DataType.Map:
                        int levelIndex = (cmsg as MapMessage).LevelIndex;

                        // load board based on index
                        FileReader levelFile = new FileReader("content/levels/multiplayer/level" + levelIndex + ".txt");
                        this.level.LoadGameBoard(levelFile.ReadGrid("level"));

                        // add bubbles and powerups to map
                        this.level.UpdateObject(cmsg);
                        break;

                    // load ghosts
                    case DataType.Ghost:
                        // ghosts should always be sent and thus received
                        // in alphabetical order. (Blinky first, Clyde second etc..)
                        
                        Ghost ghost;

                        switch (updatecount % 4)
                        {
                            case 0:
                                ghost = new Blinky();
                                break;
                            case 1:
                                ghost = new Clyde();
                                break;
                            case 2:
                                ghost = new Inky();
                                break;
                            default:
                                ghost = new Pinky();
                                break;
                        }

                        this.level.Add(ghost);
                        this.ghosts.Add(ghost);
                        updatecount++;

                        break;
                }
            }
            
            
            this.returnToLobby = false;
            this.mapUpdateTimer = MapUpdateInterval;

            this.level.Player.NetPlayer = true;
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.level.HandleInput(inputHelper);

            if (inputHelper.KeyPressed(Keys.Escape))
                this.nextState = new StateMultiplayerPaused(this, this.client, true); 
        }

        public IGameState TransitionTo()
        {
            if (!this.client.Connected)
                return new MenuErrorMessage("Lost connection to server.");
            
            if (this.returnToLobby)
                return new StateJoinLobby(this.client);

            if (this.nextState != null)
            {
                // set next state to null such that returning
                // to this state won't result into an immediate
                // transition back into pause state.
                IGameState paused;
                paused = nextState;
                nextState = null;
                return paused;
            }

            return this;
        }

        public void Update(float dt)
        {
            this.level.Update(dt);
            Console.Clear();
            Console.WriteLine(this.level.Player.Position.ToString()); 
            Console.WriteLine(this.level.Player.Lives.ToString()); 
            this.client.Update(dt);

            this.mapUpdateTimer -= dt;
            
            NetMessage send = new NetMessage();
            send.Type = PacketType.WorldState;
            this.SendData(send);
            this.client.SetData(send);

            NetMessage received = this.client.GetData();

            if (received == null)
                return;

            if (received.Type == PacketType.Lobby)
            {
                this.returnToLobby = true;
                return;
            }                
            
            this.ReceiveData(received);
        }

        public void ReceiveData(NetMessage message)
        {
            NetMessageContent cmsg;
            
            // read all messages
            while((cmsg = message.GetData()) != null)
            {
                switch(cmsg.Type)
                {
                    case DataType.Pacman:
                        if (cmsg.Id == this.client.ConnectionID)
                        {
                            PlayerMessage pmsg = cmsg as PlayerMessage;

                            Pacman ownplayer = this.level.Player;

                            if (pmsg.Lives != ownplayer.Lives)
                            {
                                if (pmsg.Lives == ownplayer.Lives - 1)
                                    ownplayer.Die();

                                else
                                    ownplayer.UpdateObject(pmsg); // out of sync, sync with server
                            }
                            
                            this.level.Player.Score = pmsg.Score;
                            continue;
                        }

                        this.players.UpdateObject(cmsg.Id, cmsg);
                        break;
                    
                    case DataType.Ghost:
                        this.ghosts.UpdateObject(cmsg);
                        break;

                    case DataType.Map:
                        // Update map only periodically to prevent
                        // triggering the bubble speed penalty twice
                        // on each and every bubble
                        if (this.mapUpdateTimer < 0)
                        {
                            this.level.UpdateObject(cmsg);
                            this.mapUpdateTimer = MapUpdateInterval;
                        }

                        break;
                }
    
            }

            this.ghosts.Restart();
        }

        public void SendData(NetMessage message)
        {
            NetMessageContent cmsg = this.client.ConstructContentMessage();

            PlayerMessage pmsg = this.level.Player.UpdateMessage(cmsg) as PlayerMessage;
            
            // setting values to -1 lets server
            // know to keep own values.
            pmsg.Lives = -1;
            pmsg.Score = -1;

            message.SetData(pmsg);
        }

        public void Draw(DrawManager drawManager)
        {
            this.level.Draw(drawManager);
        }

        public void Draw(DrawHelper drawHelper)
        {
            this.level.Draw(drawHelper);
        }
    }
}
