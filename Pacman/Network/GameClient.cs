using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class GameClient
    {
        private bool Debug = false;

        const float UpdateTimer = 1 / 32f;

        private int clientConnectionId;
        private int loginReplyRetries;
        private bool loginReplyReceived;
        private float timer;

        private NetClient client;
        private NetIncomingMessage inc;

        private NetMessage receivedData;
        private NetMessage sendData;

        public int ConnectionID
        {
            get { return this.clientConnectionId; }
        }

        public bool Connected
        {
            get;
            private set;
        }

        public GameClient()
        {
            this.loginReplyReceived = false;
            this.loginReplyRetries = 0;
            this.timer = UpdateTimer;

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.ConnectionTimeout = 15;

            this.client = new NetClient(config);
            this.client.Start();

            this.Connected = true;
        }

        public void ConnectToServer(string endpoint)
        {
            NetOutgoingMessage msg = this.client.CreateMessage();
            msg.Write((byte)0);

            this.client.Connect(endpoint, 1000, msg);
        }

        public void Disconnect()
        {
            this.client.Disconnect("leaving");
        }

        public NetMessage GetData()
        {
            NetMessage message = this.receivedData;
            this.receivedData = null;
            return message;
        }

        public void SetData(NetMessage message)
        {
            this.sendData = message;
        }

        #region NetRoutine
        private void RecieveLogin(NetMessage message)
        {
            if (Debug)
                Console.WriteLine("Received login relpy.\n" + message.ToString());

            this.clientConnectionId = message.ConnectionId;
        }

        private void ReceiveMessage(NetMessage message)
        {
            this.receivedData = message;
        }

        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            NetOutgoingMessage msg = this.client.CreateMessage();
            this.sendData.WriteMessage(msg);

            this.client.SendMessage(msg, NetDeliveryMethod.UnreliableSequenced);
            
            if (Debug)
            {
                Console.WriteLine("Message sent to server: ");
                Console.WriteLine(sendData.ToString());
                Console.WriteLine("");
            }

            this.sendData = null;
        }

        public NetMessageContent ConstructContentMessage()
        {
            NetMessageContent cmsg = new NetMessageContent();
            cmsg.Id = this.clientConnectionId;

            return cmsg;
        }

        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendMessage();
                this.timer = UpdateTimer;
            }
            
            this.inc = this.client.ReadMessage();

            if (inc == null)
                return;


            if (inc.MessageType == NetIncomingMessageType.StatusChanged)
            {
                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                this.Connected = (status != NetConnectionStatus.Disconnected);
                return;
            }

            if (inc.MessageType != NetIncomingMessageType.Data)
                return;

            NetMessage msg = new NetMessage();
            msg.ReadMessage(inc);

            if (msg.Type == PacketType.Login)
            {
                this.RecieveLogin(msg);
                this.loginReplyReceived = true;
            }

            if (!this.loginReplyReceived)
            {
                this.loginReplyRetries++;

                if (this.loginReplyRetries > 20)
                    this.Connected = false;

                return;
            }

            this.ReceiveMessage(msg);
            
            this.client.Recycle(this.inc);
        }
        #endregion
    }
}
