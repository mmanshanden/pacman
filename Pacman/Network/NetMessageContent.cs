using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Network
{
    public enum DataType
    {
        Pacman,
        Ghost,
        Bubble,
        PowerUp,
    }

    public class NetMessageContent
    {
        public DataType Type;
        public int Id;
        public int Time;


        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.Type = (DataType)msg.ReadByte();
            this.Id = msg.ReadInt32();
            this.Time = msg.ReadInt32();
        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
            msg.Write(this.Id);
            msg.Write(this.Time);
        }

        public static void CopyOver(NetMessageContent from, NetMessageContent to)
        {
            to.Type = from.Type;
            to.Id = from.Id;
            to.Time = from.Time;
        }
    }
}
