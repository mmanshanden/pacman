using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateHost : IGameState
    {
        GameServer server;
        NetPlayState send;
        Level level;

        public StateHost()
        {
            this.server = new GameServer();
            this.server.StartSimple();

            Console.Clear();
            Console.WriteLine("Hosting server");

            this.send = new NetPlayState();

            FileReader levelFile = new FileReader("Content/Levels/level1.txt");
            this.level = new Level();

            this.level.LoadGameBoard(levelFile.ReadGrid("level"));
            this.level.LoadGameBoardObjects(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Position = levelFile.ReadVector("player_position");
            this.level.Add(player);

            GhostHouse ghostHouse = new GhostHouse();
            ghostHouse.Entry = levelFile.ReadVector("ghosthouse_entry");
            ghostHouse.AddPacman(player);
            this.level.Add(ghostHouse);

            Blinky blinky = Blinky.LoadBlinky(levelFile);
            Pinky pinky = Pinky.LoadPinky(levelFile);
            Inky inky = Inky.LoadInky(levelFile);
            Clyde clyde = Clyde.LoadClyde(levelFile);

            ghostHouse.Add(blinky);
            ghostHouse.Add(clyde);
            ghostHouse.Add(inky);
            ghostHouse.Add(pinky);

            // adding self to send message
            this.send.Players.Add(new NetPlayState.Player());
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
                NetPlayState message = new NetPlayState();
                NetMessage.CopyOver(received, message);

                Console.WriteLine("Received data: ");
                Console.WriteLine(message.ToString());
                
                // the player data that was sent
                NetPlayState.Player update = message.Players[0];

                bool updated = false;

                foreach (NetPlayState.Player player in send.Players)
                {
                    // find player to update in list
                    if (player.ID != message.ConnectionId)
                        continue;

                    player.Time = message.Time;
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
                    update.Time = message.Time;
                    send.Players.Add(update);
                    Console.WriteLine("Added player to list:");
                    Console.WriteLine(update.ToString());
                }
            }

            // update self in map data
            foreach(NetPlayState.Player player in this.send.Players)
            {
                if (player.ID != 0)
                    continue;

                player.Position = this.level.Player.Position;
                player.Direction = this.level.Player.Direction;
                player.Speed = this.level.Player.Speed;
                player.Time++;
            }

            // send map data to clients
            this.server.SetData(this.send);


        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
