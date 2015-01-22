using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3dgl;

namespace Pacman
{
    class StateHostGame : IGameState
    {
        GameServer server;
        Level level;

        IndexedGameObjectList players = new IndexedGameObjectList();
        OrderedGameObjectList ghosts = new OrderedGameObjectList();

        public StateHostGame(GameServer server)
        {
            this.server = server; 

            Console.Clear();
            Console.WriteLine("Hosting server");

            this.level = this.LoadLevel(1);            

            this.players = new IndexedGameObjectList();
            this.players.Add(0, this.level.Player);
        }

        private Level LoadLevel(int index)
        {
            Level level = new Level();

            string filepath = "content/levels/multiplayer/level" + index + ".txt";
            FileReader levelfile = new FileReader(filepath);

            level = new Level();
            level.LoadGameBoard(levelfile.ReadGrid("level"));
            level.LoadGameBoardObjects(levelfile.ReadGrid("level"));

            // load player
            Player player = new Player();
            player.Spawn = levelfile.ReadVector("player1_position");
            level.Add(player);

            for (int i = 1; i <= levelfile.ReadFloat("ghosthouses"); i++)
            {
                GhostHouse ghosthouse = new GhostHouse();
                level.Add(ghosthouse);

                ghosthouse.Entry = levelfile.ReadVector("ghosthouse" + i + "_entry");

                ghosthouse.Add(Blinky.LoadBlinky(levelfile, i));
                ghosthouse.Add(Clyde.LoadClyde(levelfile, i));
                ghosthouse.Add(Inky.LoadInky(levelfile, i));
                ghosthouse.Add(Pinky.LoadPinky(levelfile, i));

                ghosthouse.SetPacman(level.Player);
            }
            
            return level;
        }
        
        public void HandleInput(InputHelper inputHelper)
        {
            this.level.HandleInput(inputHelper);
        }

        public IGameState TransitionTo()
        {
            return this;
        }

        public void Update(float dt)
        {
            this.level.Update(dt);

            NetMessage received;

            // pull messages from server
            while((received = this.server.GetData()) != null)
            {
                if (received.Type == PacketType.WorldState)
                {
                    this.ReceiveData(received);
                    continue;
                }

                if (received.Type == PacketType.Logout && 
                    this.players.Contains(received.ConnectionId))
                {
                    PlayerMessage pmsg = new PlayerMessage();
                    pmsg.Position = new Vector2(-20, -20);
                    pmsg.Lives = 0;
                    pmsg.Speed = 0;

                    // update disconnected player with fake data
                    this.players.UpdateObject(received.ConnectionId, pmsg);
                }

                Console.WriteLine(received.ConnectionId + " Disconnected");

            }

            NetMessage send = new NetMessage();
            send.Type = PacketType.WorldState;

            this.SendData(send);
            this.server.SetData(send);
        }

        public void ReceiveData(NetMessage message)
        {
            NetMessageContent cmsg;

            while((cmsg = message.GetData()) != null)
            {
                if (cmsg.Type == DataType.Pacman)
                {
                    if (!this.players.Contains(cmsg.Id))
                    {
                        // new pacman
                        Pacman pacman = new Pacman();

                        this.level.Add(pacman);
                        //this.ghostHouse.SetPacman(pacman);
                        this.players.Add(cmsg.Id, pacman);
                    }

                    this.players.UpdateObject(cmsg.Id, cmsg);
                }
            }
        }

        public void SendData(NetMessage message)
        {
            NetMessageContent baseMessage = new NetMessageContent();
            this.players.WriteAllToMessage(message, baseMessage);
            this.ghosts.WriteAllToMessage(message, baseMessage);

            MapMessage mmsg = new MapMessage();
            mmsg.Bubbles = this.level.GetBubbles();
            mmsg.PowerUps = this.level.GetPowerUps();
            message.SetData(mmsg);
        }

        public void Draw(DrawManager drawManager)
        {
            this.level.Draw(drawManager);
        }

        public void Draw(DrawHelper drawHelper)
        {

        }

    }
}
