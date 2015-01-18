using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    class StateJoin : IGameState
    {
        GameClient client;
        Level level;

        IndexedGameObjectList players;

        public StateJoin(string endpoint)
        {
            this.client = new GameClient();
            this.client.ConnectToServer(endpoint);

            Console.Clear();
            Console.WriteLine("Joining server " + endpoint);

            FileReader levelFile = new FileReader("Content/Levels/level1.txt");

            this.level = new Level();
            this.level.LoadGameBoard(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Position = levelFile.ReadVector("player_position");
            this.level.Add(player);

            this.players = new IndexedGameObjectList();
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
            this.client.Update(dt);

            NetMessage received = this.client.GetData();
            if (received != null)
                this.ReceiveData(received);

            NetMessage send = new NetMessage();
            send.Type = PacketType.WorldState;
            this.SendData(send);
            this.client.SetData(send);
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
                            continue;

                        // new player
                        if (!this.players.Contains(cmsg.Id))
                        {
                            Pacman pacman = new Pacman();
                            this.level.Add(pacman);
                            this.players.Add(cmsg.Id, pacman);
                        }

                        this.players.UpdateObject(cmsg.Id, cmsg);
                        break;

                }
    
            }
        }

        public void SendData(NetMessage message)
        {
            NetMessageContent cmsg = this.client.ConstructContentMessage();
            message.SetData(this.level.Player.UpdateMessage(cmsg));
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
