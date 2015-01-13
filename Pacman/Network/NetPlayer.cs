using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class NetPlayer
    {
        public int ID { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
        public int Time { get; set; }
        public NetConnection Connection { get; set; }
        public bool Connected { get; set; }
        public bool IsHost { get; set; }

        public NetPlayer()
        {
            this.ID = 0;
            this.Position = Vector2.Zero;
            this.Direction = Vector2.Zero;
            this.Speed = 0;
            this.Connected = true;
        }

        public void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write(ID);
            msg.Write(Position.X);
            msg.Write(Position.Y);
            msg.Write(Direction.X);
            msg.Write(Direction.Y);
            msg.Write(Speed);
            msg.Write(Time);
            msg.Write(Connected);
        }

        public void ReadMessage(NetIncomingMessage msg)
        {
            this.ID = msg.ReadInt32();
            this.Position = ReadVector(msg);
            this.Direction = ReadVector(msg);
            this.Speed = msg.ReadFloat();
            this.Time = msg.ReadInt32();
            this.Connected = msg.ReadBoolean();

            this.Connection = msg.SenderConnection;
        }

        public Vector2 ReadVector(NetIncomingMessage msg)
        {
            return new Vector2(msg.ReadFloat(), msg.ReadFloat());
        }

        public override string ToString()
        {
            string result = "";
            result += "ID: " + ID.ToString() + '\n';
            result += "Position: " + Position.ToString() + '\n';
            result += "Direction: " + Direction.ToString() + '\n';
            result += "Speed: " + Speed.ToString() + '\n';
            result += "Time: " + Time.ToString() + '\n';
            result += "Connected: " + Connected.ToString() + '\n';

            return result;

        }
    }
}
