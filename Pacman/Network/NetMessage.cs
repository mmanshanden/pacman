using Lidgren.Network;

namespace Network
{
    public class NetMessage
    {
        protected NetIncomingMessage inc;

        public PacketType PacketType;
        public DataType DataType;
        public int ConnectionId;
        public int Time;

        public NetMessage()
        {

        }
   
        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.inc = msg;

            this.PacketType = (PacketType)msg.ReadByte();
            this.DataType = (DataType)msg.ReadByte();
            this.ConnectionId = msg.ReadInt32();
            this.Time = msg.ReadInt32();
        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.PacketType);
            msg.Write((byte)this.DataType);
            msg.Write(this.ConnectionId);
            msg.Write(this.Time);
        }

        public override string ToString()
        {
            string result = "";
            result += "PacketType: " + this.PacketType.ToString() + '\n';
            result += "DataType: " + this.DataType.ToString() + '\n';
            result += "ConnectionId: " + this.ConnectionId.ToString() + '\n';
            result += "Time: " + this.Time.ToString() + '\n';

            return result;
        }

        public static void CopyOver(NetMessage from, NetMessage to)
        {
            to.inc = from.inc;
            to.inc.Position = 0;
            to.ReadMessage(to.inc);

            // critical
            to.ConnectionId = from.ConnectionId;
        }

    }
}
