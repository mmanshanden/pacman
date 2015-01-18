using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateHost : IGameState
    {
        GameServer server;
        Level level;

        IndexedGameObjectList players = new IndexedGameObjectList();

        public StateHost()
        {
            this.server = new GameServer();
            this.server.StartSimple();

            Console.Clear();
            Console.WriteLine("Hosting server");
          
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

            this.players = new IndexedGameObjectList();
            this.players.Add(0, this.level.Player);
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
                this.ReceiveData(received);
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
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
