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

        // see NetMessage.cs
        public virtual void ReadMessage(NetIncomingMessage msg)
        {
            this.Type = (DataType)msg.ReadByte();
            this.Id = msg.ReadInt32();
        }

        // see NetMessage.cs
        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
            msg.Write(this.Id);
        }

        /// <summary>
        /// Copies base content fields from one MessageContent
        /// object to the other MessageContent object.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void CopyOver(NetMessageContent from, NetMessageContent to)
        {
            to.Id = from.Id;
        }
    }
}
