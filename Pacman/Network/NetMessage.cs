using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        public DataType Type;

        public NetMessage()
        {

        }

        public void ReadMessage(NetIncomingMessage msg)
        {
            this.Type = (DataType)msg.ReadByte();
        }

        public void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

    }
}
