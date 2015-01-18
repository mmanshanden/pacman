using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Network
{
    class MapMessage : NetMessageContent
    {
        public List<Vector2> Bubbles;
        public List<Vector2> PowerUps;

        public MapMessage()
        {
            this.Type = DataType.Map;
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            int bcount = msg.ReadInt32();
            for (int i = 0; i < 32; i++)
            {
                Vector2 bubble = MessageParser.ReadVector2(msg);
                this.Bubbles.Add(bubble);
            }

            int pcount = msg.ReadInt32();
            for (int i = 0; i < pcount; i++)
            {
                Vector2 powerup = MessageParser.ReadVector2(msg);
                this.PowerUps.Add(powerup);
            }
        }

        public override void WriteMessage(NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            msg.Write(this.Bubbles.Count);
            foreach (Vector2 bubble in this.Bubbles)
                MessageParser.WriteVector2(bubble, msg);

            msg.Write(this.PowerUps.Count);
            foreach (Vector2 powerup in this.PowerUps)
                MessageParser.WriteVector2(powerup, msg);
        }
    }
}
