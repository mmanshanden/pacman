using Lidgren.Network;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Network
{
    public enum PacketType
    {
        Login,
        WorldState
    }

    class MessageParser
    {
        public static void WritePacketType(NetOutgoingMessage msg, PacketType type)
        {
            msg.Write((byte)type);
        }
        public static PacketType ReadPacketType(NetIncomingMessage msg)
        {
            return (PacketType)msg.ReadByte();
        }

        public static void WriteVector(NetOutgoingMessage msg, Vector2 vector)
        {
            msg.Write(vector.X);
            msg.Write(vector.Y);
        }

        public static Vector2 ReadVector(NetIncomingMessage msg)
        {
            return new Vector2(msg.ReadFloat(), msg.ReadFloat());
        }

        public static void WriteNetPlayerList(NetOutgoingMessage msg, List<NetPlayer> list)
        {
            msg.Write(list.Count);

            foreach (NetPlayer netplayer in list)
                netplayer.WriteMessage(msg);
        }

        public static List<NetPlayer> ReadNetPlayerList(NetIncomingMessage inc)
        {
            List<NetPlayer> result = new List<NetPlayer>();

            int count = inc.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                NetPlayer netplayer = new NetPlayer();
                netplayer.ReadMessage(inc);
                result.Add(netplayer);
            }

            return result;
        }

        public static NetOutgoingMessage CreateMessageType(NetClient client, PacketType type)
        {
            NetOutgoingMessage msg = client.CreateMessage();
            MessageParser.WritePacketType(msg, type);
            return msg;
        }
        public static NetOutgoingMessage CreateMessageType(NetServer server, PacketType type)
        {
            NetOutgoingMessage msg = server.CreateMessage();
            MessageParser.WritePacketType(msg, type);
            return msg;
        }
    }
}
