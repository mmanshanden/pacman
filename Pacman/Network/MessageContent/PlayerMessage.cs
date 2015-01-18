using Microsoft.Xna.Framework;

namespace Network
{
    public class PlayerMessage : NetMessageContent
    {
        public Vector2 Position;
        public Vector2 Direction;
        public float Speed;

        public PlayerMessage()
        {
            this.Type = DataType.Pacman;
        }

        public override void ReadMessage(Lidgren.Network.NetIncomingMessage msg)
        {
            this.Position = MessageParser.ReadVector2(msg);
            this.Direction = MessageParser.ReadVector2(msg);
            this.Speed = msg.ReadFloat();
        }

        public override void WriteMessage(Lidgren.Network.NetOutgoingMessage msg)
        {
            MessageParser.WriteVector2(this.Position, msg);
            MessageParser.WriteVector2(this.Direction, msg);
            msg.Write(Speed);

        }
    }
}
