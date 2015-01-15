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

        List<Pacman> players;

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

            this.players = new List<Pacman>();
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

            NetPlayState.Player self = new NetPlayState.Player();
            self.Position = this.level.Player.Position;
            self.Direction = this.level.Player.Direction;
            self.Speed = this.level.Player.Speed;

            NetPlayState send = new NetPlayState();
            send.Players.Add(self);

            this.client.SetData(send);

            // get last received server message
            NetMessage received = this.client.GetData();
            
            if (received == null)
                return;

            if (received.DataType != DataType.Playing)
                return;

            // convert to playing state message
            NetPlayState worldstate = new NetPlayState();
            NetMessage.CopyOver(received, worldstate);

            foreach (NetPlayState.Player update in worldstate.Players)
            {
                if (update.ID == this.client.ConnectionID)
                    continue;

                bool updated = false;

                foreach(Pacman netplayer in this.players)
                {
                    if (update.ID != netplayer.NetData.ID)
                        continue;

                    updated = true;

                    if (update.Time <= netplayer.NetData.Time)
                        continue;

                    netplayer.NetData = update;

                    netplayer.Position = update.Position;
                    netplayer.Direction = update.Direction;
                    netplayer.Speed = update.Speed;

                    // setting updated to true here in incorrect
                    // this will create a new player if the time
                    // was not updated
                    // updated = true;
                }

                // new player
                if (!updated)
                {
                    Pacman newplayer = new Pacman();
                    newplayer.Position = update.Position;
                    newplayer.Direction = update.Direction;
                    newplayer.Speed = update.Speed;
                    newplayer.NetData = update;

                    this.players.Add(newplayer);
                    this.level.Add(newplayer);
                    Console.WriteLine("New player added");
                }

            }

        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
