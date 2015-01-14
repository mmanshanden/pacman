using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        NetIncomingMessage inc;
        public DataType Type;

        public NetMessage()
        {

        }

        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.Type = (DataType)msg.ReadByte();
        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

    }
}
