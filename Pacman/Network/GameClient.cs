using Lidgren.Network;
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

        #region NetRoutine
        private void RecieveLogin()
        {
            string message = inc.ReadString();
            Console.WriteLine("Login message: " + message);
        }

        private void ReceiveMessage()
        {
            
        }

        private void SendWorldState()
        {
            
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

            }

        }
        #endregion
    }
}
