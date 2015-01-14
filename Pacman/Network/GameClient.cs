using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class GameClient
    {
        const float UpdateTimer = 3f;

        private int clientUpdateCount;
        private bool loginReplyReceived;
        private float timer;

        private NetClient client;
        private NetIncomingMessage inc;

        private NetMessage receivedData;
        private NetMessage sendData;

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
            this.sendData = message;
        }

        #region NetRoutine
        private void RecieveLogin()
        {
            string message = inc.ReadString();
            Console.WriteLine("Login message: " + message);
        }

        private void ReceiveMessage()
        {
            this.receivedData = new NetMessage();
            this.receivedData.ReadMessage(inc);
            Console.WriteLine(this.receivedData.ToString());
        }

        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            NetOutgoingMessage msg = this.client.CreateMessage();
            this.sendData.WriteMessage(msg);
            this.client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            this.sendData = null;
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

            PacketType packet = (PacketType)inc.ReadByte();

            if (packet == PacketType.Login)
            {
                this.RecieveLogin();
                this.loginReplyReceived = true;
            }

            if (!this.loginReplyReceived)
                return;

            switch (packet)
            {
                case PacketType.WorldState:
                    this.ReceiveMessage();
                    break;
            }

        }
        #endregion
    }
}
