using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Collections.Generic;

namespace Network
{
    public class PlayingMessage : NetMessage
    {
        public class Player
        {
            public int ID;
            public Vector2 Position;
            public Vector2 Direction;
            public float Speed;

            public void WriteMessage(NetOutgoingMessage msg)
            {
                msg.Write(this.ID);
                MessageParser.WriteVector2(this.Position, msg);
                MessageParser.WriteVector2(this.Direction, msg);
                msg.Write(Speed);
            }
            public void ReadMessage(NetIncomingMessage msg)
            {
                this.ID = msg.ReadInt32();
                this.Position = MessageParser.ReadVector2(msg);
                this.Direction = MessageParser.ReadVector2(msg);
                this.Speed = msg.ReadFloat();
            }
            public override string ToString()
            {
                string result = "";
                result += "ID: " + ID.ToString() + '\n';
                result += "Position: " + Position.ToString() + '\n';
                result += "Direction: " + Direction.ToString() + '\n';
                result += "Speed: " + Speed.ToString() + '\n';

                return result;
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
                MessageParser.WriteVector2(this.Position, msg);
                MessageParser.WriteVector2(this.Direction, msg);
                msg.Write(Speed);
                MessageParser.WriteVector2(this.Target, msg);
            }
            public void ReadMessage(NetIncomingMessage msg)
            {
                this.Position = MessageParser.ReadVector2(msg);
                this.Direction = MessageParser.ReadVector2(msg);
                this.Speed = msg.ReadFloat();
                this.Target = MessageParser.ReadVector2(msg);
            }
        }

        public List<Player> Players;
        public List<Ghost> Ghosts;

        public PlayingMessage()
        {
            base.PacketType = PacketType.WorldState;
            base.DataType = DataType.Playing;

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

            int playercount = inc.ReadInt32();
            for (int i = 0; i < playercount; i++)
            {
                Player player = new Player();
                player.ReadMessage(inc);
                this.Players.Add(player);
            }

            int ghostcount = inc.ReadInt32();
            for (int i = 0; i < ghostcount; i++)
            {
                Ghost ghost = new Ghost();
                ghost.ReadMessage(inc);
                this.Ghosts.Add(ghost);
            }

        }

        public override string ToString()
        {
            string result = base.ToString();

            result += "Players:\n";
            foreach (Player player in this.Players)
                result += player.ToString();

            return result;
        }

    }
}
