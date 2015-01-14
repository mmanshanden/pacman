using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class GameClient
    {
        const float UpdateTimer = 3f;

        private int clientConnectionId;
        private int clientUpdateCount;
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

        public GameClient()
        {
            this.loginReplyReceived = false;
            this.clientUpdateCount = 0;
            this.timer = UpdateTimer;

            NetPeerConfiguration config = new NetPeerConfiguration("game");

            this.client = new NetClient(config);
            this.client.Start();
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
            this.clientConnectionId = message.ConnectionId;
            this.sendData = message;
        }

        #region NetRoutine
        private void RecieveLogin(NetMessage message)
        {
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

            this.client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            this.sendData = null;

            Console.WriteLine("message send to server");
        }

        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendMessage();
                this.clientUpdateCount++;
                this.timer = UpdateTimer;
            }
            
            this.inc = this.client.ReadMessage();

            if (inc == null)
                return;

            if (inc.MessageType != NetIncomingMessageType.Data)
                return;

            NetMessage msg = new NetMessage();
            msg.ReadMessage(inc);

            if (msg.PacketType == PacketType.Login)
            {
                this.RecieveLogin(msg);
                this.loginReplyReceived = true;
            }

            if (!this.loginReplyReceived)
                return;

            switch (msg.PacketType)
            {
                case PacketType.WorldState:
                    this.ReceiveMessage(msg);
                    break;
            }

        }
        #endregion
    }
}
