using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateHost : IGameState
    {
        GameServer server;
        PlayingMessage send;
        Level level;

        public StateHost()
        {
            this.server = new GameServer();
            this.server.StartSimple();

            Console.Clear();
            Console.WriteLine("Hosting server");

            this.send = new PlayingMessage();

            FileReader levelFile = new FileReader("Content/Levels/level1.txt");
            
            this.level = new Level();
            this.level.LoadGameBoard(levelFile.ReadGrid("level"));
            this.level.LoadGameBoardObjects(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Position = levelFile.ReadVector("player_position");
            this.level.Add(player);

            GhostHouse ghostHouse = new GhostHouse();
            this.level.Add(ghostHouse);

            Blinky blinky = new Blinky(player);
            blinky.Position = levelFile.ReadVector("blinky_position");
            blinky.Direction = Vector2.UnitX;
            ghostHouse.Add(blinky);

            // adding self to send message
            this.send.Players.Add(new PlayingMessage.Player());
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
            this.server.Update(dt);
            
            NetMessage received;

            // pull data from server
            while ((received = this.server.GetData()) != null)
            {
                if (received.DataType != DataType.Playing)
                    continue;

                // convert to playingmessage
                PlayingMessage message = new PlayingMessage();
                NetMessage.CopyOver(received, message);
                
                // the player data that was sent
                PlayingMessage.Player update = message.Players[0];

                bool updated = false;

                foreach (PlayingMessage.Player player in send.Players)
                {
                    // find player to update in list
                    if (player.ID != message.ConnectionId)
                        continue;
                    
                    player.Position = update.Position;
                    player.Direction = update.Direction;
                    player.Speed = update.Speed;

                    Console.WriteLine("Updated player " + player.ID);

                    updated = true;
                }

                // add as new player
                if (!updated)
                {
                    update.ID = message.ConnectionId;
                    send.Players.Add(update);
                    Console.WriteLine("Added player to list:");
                    Console.WriteLine(update.ToString());
                }
            }

            // update self in map data
            foreach(PlayingMessage.Player player in this.send.Players)
            {
                if (player.ID != 0)
                    continue;

                player.Position = this.level.Player.Position;
                player.Direction = this.level.Player.Direction;
                player.Speed = this.level.Player.Speed;
            }

            // send map data to clients
            this.server.SetData(this.send);

            /* EXAMPLE
            // prepare data for clients
            PlayingMessage send = new PlayingMessage();
            
            // begin arbitrary data
            PlayingMessage.Player player = new PlayingMessage.Player();
            player.Position = new Vector2(0.02134f, 12.12403f);
            player.Speed = 9001;
            send.Players.Add(player);

            PlayingMessage.Ghost ghost = new PlayingMessage.Ghost();
            ghost.Direction = new Vector2(1, 0);
            ghost.Target = new Vector2(6, 9);
            send.Ghosts.Add(ghost);
            // read all messages from server

            // tell server to send it when its timer elapses
            this.server.SetData(send);

            
            // get data that was sent back to server
            // i.e. the data the server received;
            NetMessage received;

            // pull messages until theres none left
            while((received = this.server.GetData()) != null) 
            {
                // this is the message that was received by the server;
                Console.WriteLine("Message received from client:");
                Console.WriteLine(received.ToString());
                Console.WriteLine("");
            }
            */
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
