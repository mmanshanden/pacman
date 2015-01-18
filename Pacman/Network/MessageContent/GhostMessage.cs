using Microsoft.Xna.Framework;

namespace Network
{
    public class GhostMessage : NetMessageContent
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;
        public Vector2 Target;

        public GhostMessage()
        {
            this.Type = DataType.Ghost;
        }

        public override void ReadMessage(Lidgren.Network.NetIncomingMessage msg)
        {
            base.ReadMessage(msg);

            this.Position = MessageParser.ReadVector2(msg);
            this.Direction = MessageParser.ReadVector2(msg);
            this.Speed = msg.ReadFloat();
            this.Target = MessageParser.ReadVector2(msg);
        }

        public override void WriteMessage(Lidgren.Network.NetOutgoingMessage msg)
        {
            base.WriteMessage(msg);

            MessageParser.WriteVector2(this.Position, msg);
            MessageParser.WriteVector2(this.Direction, msg);
            msg.Write(Speed);
            MessageParser.WriteVector2(this.Target, msg);
        }
    }
}
