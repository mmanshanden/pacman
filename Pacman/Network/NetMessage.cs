using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        protected NetIncomingMessage inc;
        public DataType Type;

        public NetMessage()
        {

        }
        public NetMessage(NetIncomingMessage inc)
        {
            this.inc = inc;
        }
        public NetMessage(NetMessage msg)
        {
            this.inc = msg.inc;
        }

        public void ReadHeaders()
        {
            this.Type = (DataType)this.inc.ReadByte();
        }

        public virtual void Parse()
        {

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
