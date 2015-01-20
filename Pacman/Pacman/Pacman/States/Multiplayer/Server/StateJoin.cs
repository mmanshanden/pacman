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
        OrderedGameObjectList ghosts;

        GameObjectList bubbles;
        GameObjectList powerups;

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
            player.Spawn = levelFile.ReadVector("player_position");
            this.level.Add(player);

            this.players = new IndexedGameObjectList();
            this.ghosts = new OrderedGameObjectList();

            this.bubbles = new GameObjectList();
            this.powerups = new GameObjectList();

            this.level.Add(bubbles);
            this.level.Add(powerups);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.level.HandleInput(inputHelper);
        }

        public IGameState TransitionTo()
        {
            if (!this.client.Connected)
                return new MenuMultiplayer();
                
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


            int updatecount = 0;

            // read all messages
            while((cmsg = message.GetData()) != null)
            {
                switch(cmsg.Type)
                {
                    case DataType.Pacman:
                        if (cmsg.Id == this.client.ConnectionID)
                        {
                            PlayerMessage pmsg = cmsg as PlayerMessage;

                            this.level.Player.Lives = pmsg.Lives;
                            this.level.Player.Score = pmsg.Score;
                            continue;
                        }

                        // new player
                        if (!this.players.Contains(cmsg.Id))
                        {
                            Pacman pacman = new Pacman();
                            this.level.Add(pacman);
                            this.players.Add(cmsg.Id, pacman);
                        }

                        this.players.UpdateObject(cmsg.Id, cmsg);
                        break;
                    
                    case DataType.Ghost:
                        if (updatecount == this.ghosts.Count)
                        {
                            Ghost ghost;

                            switch(updatecount % 4)
                            {
                                case 1:
                                    ghost = new Clyde();
                                    break;
                                case 2:
                                    ghost = new Inky();
                                    break;
                                case 3:
                                    ghost = new Pinky();
                                    break;
                                default:
                                    ghost = new Blinky();
                                    break;
                            }

                            this.level.Add(ghost);
                            this.ghosts.Add(ghost);
                            updatecount++;
                        }

                        this.ghosts.UpdateObject(cmsg);
                        break;

                    case DataType.Map:
                        MapMessage mmsg = cmsg as MapMessage;

                        this.bubbles.Clear();
                        this.powerups.Clear();

                        foreach (Vector2 bubble in mmsg.Bubbles)
                        {
                            Bubble b = new Bubble();
                            b.Position = bubble;
                            this.bubbles.Add(b);
                        }

                        foreach (Vector2 powerup in mmsg.PowerUps)
                        {
                            Powerup p = new Powerup();
                            p.Position = powerup;
                            this.powerups.Add(p);
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

            message.SetData(this.level.Player.UpdateMessage(cmsg));
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);

            Console.Clear();
            Console.WriteLine("Lives:");
            Console.WriteLine(this.level.Player.Lives.ToString());
            Console.WriteLine("Score:");
            Console.WriteLine(this.level.Player.Score.ToString());
        }

    }
}
