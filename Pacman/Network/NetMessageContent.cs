using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Network
{
    public enum DataType
    {
        Pacman,
        Ghost,
        Map,
        Lobby,
    }

    public class NetMessageContent
    {
        public DataType Type;
        public int Id;


        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.Type = (DataType)msg.ReadByte();
            this.Id = msg.ReadInt32();
        }

        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
            msg.Write(this.Id);
        }

        public static void CopyOver(NetMessageContent from, NetMessageContent to)
        {
            to.Id = from.Id;
        }
    }
}
