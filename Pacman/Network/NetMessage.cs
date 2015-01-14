using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        public DataType Type;

        public NetMessage()
        {

        }

        public virtual void ReadMessage(NetIncomingMessage msg)
        {

        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            
        }

        public override string ToString()
        {
            return this.Type.ToString();
        }

    }
}
