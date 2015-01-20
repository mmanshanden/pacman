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

        public virtual void ReadMessage(NetIncomingMessage msg)
        {            
            this.Type = (PacketType)msg.ReadByte();
            this.ConnectionId = msg.ReadInt32();

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
        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
            msg.Write(this.ConnectionId);

            foreach (NetMessageContent cmsg in this.content) 
            {
                cmsg.WriteMessage(msg);
            }
        }

        public NetMessageContent GetData()
        {
            if (index >= this.content.Count)
                return null;

            NetMessageContent cmsg = this.content[index];
            this.index++;

            return cmsg;
        }
        public void SetData(NetMessageContent cmsg)
        {
            this.content.Add(cmsg);
        }

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
