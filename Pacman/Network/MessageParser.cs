using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace Network
{
    public enum PacketType
    {
        Login,
        WorldState
    }

    public enum DataType
    {
        Lobby,
        Start,
        Playing
    }


    public class MessageParser
    {
        public static void WriteVector2(Vector2 vector, NetOutgoingMessage msg)
        {
            msg.Write(vector.X);
            msg.Write(vector.Y);
        }
        public static Vector2 ReadVector2(NetIncomingMessage msg)
        {
            return new Vector2(msg.ReadFloat(), msg.ReadFloat());
        }



    }
}
