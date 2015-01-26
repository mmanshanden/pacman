using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class LobbyMessage : NetMessageContent
    {
        public float Rotation;
        public int Score;

        public LobbyMessage()
        {
            this.Type = DataType.Lobby;
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            this.Rotation = msg.ReadFloat();
            this.Score = msg.ReadInt32();
        }

        public override void WriteMessage(NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            msg.Write(this.Rotation);
            msg.Write(this.Score);
        }
    }
}
