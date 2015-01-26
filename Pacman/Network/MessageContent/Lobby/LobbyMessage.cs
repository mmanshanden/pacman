using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class LobbyMessage : NetMessageContent
    {
        public int PlayerCount;

        public float Rotation;
        public int Name;
        public int Score;

        public LobbyMessage()
        {
            this.Type = DataType.Lobby;
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            this.PlayerCount = msg.ReadInt32();
            this.Rotation = msg.ReadFloat();
            this.Name = msg.ReadInt32();
            this.Score = msg.ReadInt32();
        }

        public override void WriteMessage(NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            msg.Write(this.PlayerCount);
            msg.Write(this.Rotation);
            msg.Write(this.Name);
            msg.Write(this.Score);
        }
    }
}
