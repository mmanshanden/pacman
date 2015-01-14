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

        private DataType dataType;
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

        public void SetData(DataType dataType, NetMessage message)
        {
            this.dataType = dataType;
            this.sendData = message;
        }

        #region NetRoutine
        private void RecieveLogin()
        {
            string message = inc.ReadString();
            Console.WriteLine("Login message: " + message);
        }

        private void ReceiveMessage(DataType type)
        {
            this.receivedData = new NetMessage();

            switch (type)
            {
                case DataType.Playing:
                    Console.WriteLine("Received PlayingMessage");
                    this.receivedData = new PlayingMessage();
                    break;
            }

            this.receivedData.ReadMessage(inc);

        }

        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            NetOutgoingMessage msg = this.client.CreateMessage();
            msg.Write((byte)PacketType.WorldState);
            msg.Write((byte)this.dataType);

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
                    DataType dataType = (DataType)inc.ReadByte();
                    this.ReceiveMessage(dataType);
                    break;
            }

        }
        #endregion
    }
}
