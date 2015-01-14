using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class GameClient
    {
        const float UpdateTimer = 3f;

        private int clientNetworkId;
        private int clientUpdateCount;
        private bool loginReplyReceived;
        private float timer;

        private NetClient client;
        private NetIncomingMessage inc;

        public GameClient()
        {
            this.loginReplyReceived = false;
            this.clientUpdateCount = 0;
            this.timer = UpdateTimer;

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            this.client = new NetClient(config);
            this.client.Start();
        }

        public void ConnectToServer(string endpoint)
        {
            NetOutgoingMessage msg = MessageParser.CreateMessageType(client, PacketType.Login);
            this.client.Connect(endpoint, 1000, msg);
        }
        public void Disconnect()
        {
            this.client.Disconnect("leaving");
        }

        #region NetRoutine
        private void RecieveLogin()
        {
            this.clientNetworkId = inc.ReadInt32();
        }

        private void ReceiveMessage(PacketType type)
        {
            switch (type)
            {
                case PacketType.WorldState:
                    this.receivedData = MessageParser.ReadNetPlayerList(inc);
                    break;
            }
        }

        private void SendWorldState()
        {
            if (sendData == null)
                return;

            // prepare message
            NetOutgoingMessage msg = client.CreateMessage();
            MessageParser.WritePacketType(msg, PacketType.WorldState);
            MessageParser.WriteNetPlayerList(msg, this.sendData);

            // send message
            Console.WriteLine("CLIENT | Sending WorldState!");
            this.client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);

            // update count
            this.clientUpdateCount++;
        }

        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendWorldState();
                this.timer = UpdateTimer;
            }
            
            this.inc = this.client.ReadMessage();

            if (inc == null)
                return;

            if (inc.MessageType != NetIncomingMessageType.Data)
                return;

            PacketType type = (PacketType)inc.ReadByte();

            if (type == PacketType.Login)
            {
                this.RecieveLogin();
                this.loginReplyReceived = true;
                return;
            }

            if (!this.loginReplyReceived)
                return;

            this.ReceiveMessage(type);

            
            this.sendData = null;
        }
        #endregion
    }
}
