using Lidgren.Network;
using System.Collections.Generic;

namespace Network
{
    public enum PacketType
    {
        Login,
        WorldState
    }

    public class NetMessage
    {
        private int index = 0;

        public PacketType Type;
        public int ConnectionId;
        public int Time;

        List<NetMessageContent> content;

        public NetMessage()
        {
            this.content = new List<NetMessageContent>();
        }
   
        public virtual void ReadMessage(NetIncomingMessage msg)
        {            
            this.Type = (PacketType)msg.ReadByte();
            this.ConnectionId = msg.ReadInt32();
            this.Time = msg.ReadInt32();

            while(msg.Position != msg.LengthBits)
            {
                DataType type = (DataType)msg.ReadByte();
                NetMessageContent c = new NetMessageContent();

                // content reads byte again
                msg.Position -= 8;

                switch (type)
                {
                    case DataType.Pacman:
                        c = new PlayerMessage();
                        break;
                    case DataType.Ghost:
                        c = new GhostMessage();
                        break;
                }

                c.ReadMessage(msg);
                this.content.Add(c);
            }
        }
        public virtual void WriteMessage(NetOutgoingMessage msg)
        {
            msg.Write((byte)this.Type);
            msg.Write(this.ConnectionId);
            msg.Write(this.Time);

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
            result += "Time: " + this.Time.ToString() + '\n';
            result += "Contents: " + this.content.Count.ToString() + '\n';

            return result;
        }

    }
}
