using Lidgren.Network;
using System.Collections.Generic;

namespace Network
{
    public enum PacketType
    {
        Login,
        Logout,
        Lobby,
        WorldState
    }

    /// <summary>
    /// A message contains content following a predefined
    /// structure. Content can written to a
    /// netmessage which then gets send over the network.
    /// On the receiving end, that same content can be 
    /// read again.
    /// </summary>
    public class NetMessage
    {
        private int index = 0;

        public PacketType Type;
        public int ConnectionId;

        List<NetMessageContent> content;

        public NetMessage()
        {
            this.content = new List<NetMessageContent>();
        }

        /// <summary>
        /// Returns the appropiate content type based on datatype
        /// for message of packet type "WorldState".
        /// </summary>
        /// <param name="type">Datatype inside worldstate packet</param>
        /// <returns>Content type</returns>
        private NetMessageContent ParseWorldState(DataType type)
        {
            switch (type)
            {
                case DataType.Pacman:
                    return new PlayerMessage();
                case DataType.Ghost:
                    return new GhostMessage();
                case DataType.Map:
                    return new MapMessage();
            }

            return new NetMessageContent();
        }

        /// <summary>
        /// Reads and parses network data.
        /// </summary>
        /// <param name="msg">Network data</param>
        public virtual void ReadMessage(NetIncomingMessage msg)
        {            
            this.Type = (PacketType)msg.ReadByte();
            this.ConnectionId = msg.ReadInt32();

            // read until end reached
            while(msg.Position != msg.LengthBits)
            {
                DataType type = (DataType)msg.ReadByte();
                NetMessageContent c = new NetMessageContent();

                // content reads byte again
                msg.Position -= 8;

                switch (this.Type)
                {
                    case PacketType.WorldState:
                        c = this.ParseWorldState(type);
                        break;

                    case PacketType.Lobby:
                        c = new LobbyMessage();
                        break;
                }
                
                // read and add message
                c.ReadMessage(msg);
                this.content.Add(c);
            }
        }

        /// <summary>
        /// Convert content to network data.
        /// </summary>
        /// <param name="msg">Network data container</param>
        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type); // cast enums to byte
            msg.Write(this.ConnectionId);

            foreach (NetMessageContent cmsg in this.content) 
            {
                cmsg.WriteMessage(msg);
            }
        }

        /// <summary>
        /// Get first, unread, entry in content list.
        /// </summary>
        /// <returns>Message content</returns>
        public NetMessageContent GetData()
        {
            if (index >= this.content.Count)
                return null;

            NetMessageContent cmsg = this.content[index];
            this.index++;

            return cmsg;
        }

        /// <summary>
        /// Adds content to content list
        /// </summary>
        /// <param name="cmsg">Message content</param>
        public void SetData(NetMessageContent cmsg)
        {
            this.content.Add(cmsg);
        }

        /// <summary>
        /// Writes message data to string
        /// </summary>
        /// <returns>Message data</returns>
        public override string ToString()
        {
            string result = "";
            result += "PacketType: " + this.Type.ToString() + '\n';
            result += "DataType: " + this.Type.ToString() + '\n';
            result += "ConnectionId: " + this.ConnectionId.ToString() + '\n';
            result += "Contents: " + this.content.Count.ToString() + '\n';

            return result;
        }

    }
}
