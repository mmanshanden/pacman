using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3dgl;
using System.Collections.Generic;

namespace Pacman
{
    class StateHostGame : IGameState
    {
        GameServer server;
        Level level;

        IndexedGameObjectList players;
        OrderedGameObjectList ghosts;

        public StateHostGame(GameServer server)
        {
            this.server = server; 

            Console.Clear();
            Console.WriteLine("Hosting server");
          
            this.players = new IndexedGameObjectList();
            this.ghosts = new OrderedGameObjectList();
            
            this.level = this.LoadLevel(1);
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

            List<Pacman> ghosthousetargets = new List<Pacman>();

            // load self
            Player player = new Player();
            player.Spawn = levelfile.ReadVector("player1_position");
            ghosthousetargets.Add(player);
            level.Add(player);

            // load other players
            for (int i = 2; i <= levelfile.ReadFloat("players"); i++)
            {
                Pacman pacman = new Pacman();
                pacman.Spawn = levelfile.ReadVector("player" + i + "_position");
                this.players.Add(this.server.GetConnectedIDs()[i - 2], pacman);
                ghosthousetargets.Add(pacman);
                level.Add(pacman);

            }
            
            for (int i = 1; i <= levelfile.ReadFloat("ghosthouses"); i++)
            {
                GhostHouse ghosthouse = new GhostHouse();
                level.Add(ghosthouse);

                ghosthouse.Entry = levelfile.ReadVector("ghosthouse" + i + "_entry");

                Blinky blinky = Blinky.LoadBlinky(levelfile, i);
                ghosthouse.Add(blinky);
                this.ghosts.Add(blinky);

                Clyde clyde = Clyde.LoadClyde(levelfile, i);
                ghosthouse.Add(clyde);
                this.ghosts.Add(clyde);

                Inky inky = Inky.LoadInky(levelfile, i);
                ghosthouse.Add(inky);
                this.ghosts.Add(inky);

                Pinky pinky = Pinky.LoadPinky(levelfile, i);
                ghosthouse.Add(pinky);
                this.ghosts.Add(pinky);

                Pacman target = ghosthousetargets[i % ghosthousetargets.Count];
                ghosthouse.SetPacman(target);
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
            this.server.Update(dt);

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
                    this.players.UpdateObject(cmsg.Id, cmsg);
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
            this.level.Draw(drawHelper);
        }

    }
}
