using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Network
{
    public class PlayerMessage : NetMessageContent
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public int Lives;
        public int Score;

        public PlayerMessage()
        {
            this.Type = DataType.Pacman;
        }

        public override void ReadMessage(NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            this.Position = MessageParser.ReadVector2(msg);
            this.Direction = MessageParser.ReadVector2(msg);
            this.Speed = msg.ReadFloat();
            this.Lives = msg.ReadInt32();
            this.Score = msg.ReadInt32();
        }

        public override void WriteMessage(NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            MessageParser.WriteVector2(this.Position, msg);
            MessageParser.WriteVector2(this.Direction, msg);
            msg.Write(Speed);
            msg.Write(Lives);
            msg.Write(Score);

        }
    }
}
