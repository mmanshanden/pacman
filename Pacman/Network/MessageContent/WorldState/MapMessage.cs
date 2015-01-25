using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Network
{
    /// <summary>
    /// Used as a container for map player data
    /// across the network.
    /// </summary>
    public class MapMessage : NetMessageContent
    {
        public int LevelIndex;
        public List<Vector2> Bubbles;
        public List<Vector2> PowerUps;

        public MapMessage()
        {
            this.Type = DataType.Map;

            this.Bubbles = new List<Vector2>();
            this.PowerUps = new List<Vector2>();
        }

        public MapMessage(int levelIndex)
        {
            this.Type = DataType.Map;

            this.LevelIndex = levelIndex;
            this.Bubbles = new List<Vector2>();
            this.PowerUps = new List<Vector2>();
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            this.LevelIndex = msg.ReadInt32();

            int bcount = msg.ReadInt32();
            for (int i = 0; i < bcount; i++)
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

            msg.Write(this.LevelIndex);

            msg.Write(this.Bubbles.Count); // write count for read
            foreach (Vector2 bubble in this.Bubbles)
                MessageParser.WriteVector2(bubble, msg);

            msg.Write(this.PowerUps.Count);
            foreach (Vector2 powerup in this.PowerUps)
                MessageParser.WriteVector2(powerup, msg);
        }
    }
}
