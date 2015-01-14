using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        protected NetIncomingMessage inc;

        public PacketType PacketType;
        public DataType DataType;

        public NetMessage()
        {

        }
   
        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.inc = msg;

            this.PacketType = (PacketType)msg.ReadByte();
            this.DataType = (DataType)msg.ReadByte();
        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.PacketType);
            msg.Write((byte)this.DataType);
        }

        public override string ToString()
        {
            return this.DataType.ToString();
        }

        public static void CopyOver(NetMessage from, NetMessage to)
        {
            to.inc = from.inc;
            to.inc.Position = 0;
            to.ReadMessage(to.inc);
        }

    }
}
