using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Collections.Generic;

namespace Network
{
    public class PlayingMessage : NetMessage
    {
        public class Player
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Speed;

            public void WriteMessage(NetOutgoingMessage msg)
            {
                msg.Write(this.Position.X);
                msg.Write(this.Position.Y);
                msg.Write(this.Direction.X);
                msg.Write(this.Direction.Y);
                msg.Write(Speed);
            }
            public void ReadMessage(NetIncomingMessage msg)
            {
                this.Position.X = msg.ReadFloat();
                this.Position.Y = msg.ReadFloat();
                this.Direction.X = msg.ReadFloat();
                this.Direction.Y = msg.ReadFloat();
                this.Speed = msg.ReadFloat();
            }
        }
        public class Ghost
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Speed;
            public Vector2 Target;

            public void WriteMessage(NetOutgoingMessage msg)
            {
                msg.Write(this.Position.X);
                msg.Write(this.Position.Y);
                msg.Write(this.Direction.X);
                msg.Write(this.Direction.Y);
                msg.Write(Speed);
                msg.Write(this.Target.X);
                msg.Write(this.Target.Y);
            }

            public void ReadMessage(NetIncomingMessage msg)
            {
                this.Position.X = msg.ReadFloat();
                this.Position.Y = msg.ReadFloat();
                this.Direction.X = msg.ReadFloat();
                this.Direction.Y = msg.ReadFloat();
                this.Speed = msg.ReadFloat();
                this.Target.X = msg.ReadFloat();
                this.Target.Y = msg.ReadFloat();
            }
        }

        public List<Player> Players;
        public List<Ghost> Ghosts;

        public PlayingMessage()
        {
            this.Type = DataType.Playing;

            this.Players = new List<Player>();
            this.Ghosts = new List<Ghost>();
        }

        public override void WriteMessage(NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            msg.Write(this.Players.Count);
            foreach (Player player in this.Players)
                player.WriteMessage(msg);

            msg.Write(this.Ghosts.Count);
            foreach (Ghost ghost in this.Ghosts)
                ghost.WriteMessage(msg);
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            int playercount = msg.ReadInt32();
            for (int i = 0; i < playercount; i++)
            {
                Player player = new Player();
                player.ReadMessage(msg);
                this.Players.Add(player);
            }

            int ghostcount = msg.ReadInt32();
            for (int i = 0; i < ghostcount; i++)
            {
                Ghost ghost = new Ghost();
                ghost.ReadMessage(msg);
                this.Ghosts.Add(ghost);
            }
        }


    }
}
