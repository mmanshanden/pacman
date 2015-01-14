using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Network
{
    public class GameServer
    {
        const float UpdateTimer = 3f;
        const int ServerPort = 1000;

        private Thread serverThread;
        private bool serverRunning;
        private bool serverStarted;
        private float timer;

        private NetServer server;
        private NetIncomingMessage inc;

        private NetMessage sendData;
        private List<NetMessage> receivedData;

        public GameServer()
        {
            this.serverRunning = false;
            this.serverStarted = false;
            this.timer = UpdateTimer;
            

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.MaximumConnections = 8;
            config.Port = ServerPort;

            this.server = new NetServer(config);

            this.receivedData = new List<NetMessage>();
        }
        
        public void StartSimple()
        {
            this.server.Start();
            this.serverStarted = true;
        }

        public void SetData(NetMessage netMessage)
        {
            this.sendData = netMessage;
        }
        public NetMessage GetData()
        {
            if (this.receivedData.Count == 0)
                return null;

            NetMessage msg = this.receivedData[0];
            this.receivedData.Remove(msg);
            return msg;
        }

        #region Threading
        public void Start()
        {
            if (this.serverStarted)
                throw new Exception("Cannot start server more than once.");

            this.server.Start();

            // allow start up time
            Thread.Sleep(200);

            this.serverThread = new Thread(new ThreadStart(Run));
            this.serverThread.IsBackground = true;
            this.serverThread.Start();
        }

        public void Stop()
        {
            if (!this.serverStarted)
                return;

            this.server.Shutdown("server stopped");
            this.serverRunning = false;
        }

        private void Run()
        {
            DateTime time = DateTime.Now;
            while (this.serverRunning)
            {
                DateTime now = DateTime.Now;
                float dt = (float)(now - time).TotalSeconds;
                time = now;

                this.Update(dt);
            }
        }
        
        #endregion

        private void ReceiveDiscoveryRequest()
        {
            NetOutgoingMessage respone = this.server.CreateMessage();
            respone.Write("server");
            respone.Write(server.Connections.Count);

            server.SendDiscoveryResponse(respone, inc.SenderEndpoint);
        }

        private void ReceiveLogin()
        {
            // Approve connection (critical)
            inc.SenderConnection.Approve();

            // reply
            NetOutgoingMessage msg = server.CreateMessage();
            msg.Write((byte)PacketType.Login);
            msg.Write("Connection approved");

            // allow start up time
            Thread.Sleep(200);

            // send reply
            server.SendMessage(msg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
            Console.WriteLine("Login reply send back to " + inc.SenderEndpoint.Address.ToString());
        }

        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            if (this.server.Connections.Count == 0)
                return;

            Console.WriteLine("Sending " + this.sendData.ToString());

            // message
            NetOutgoingMessage msg = this.server.CreateMessage();
            msg.Write((byte)PacketType.WorldState);
            this.sendData.WriteMessage(msg);

            // send
            this.server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

            // message has been send, dont send again
            this.sendData = null;
        }
        private void ReceiveMessage()
        {
            NetMessage msg = new NetMessage();
            msg.ReadMessage(inc);

            this.receivedData.Add(msg);
        }

        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendMessage();
                this.timer = UpdateTimer;
            }

            this.inc = server.ReadMessage();

            if (inc == null)
                return;

            switch (inc.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    this.ReceiveDiscoveryRequest();
                    break;
                case NetIncomingMessageType.ConnectionApproval:
                    this.ReceiveLogin();
                    break;
                case NetIncomingMessageType.Data:
                    if ((PacketType)inc.ReadByte() == PacketType.WorldState)
                        this.ReceiveMessage();

                    break;
            }
        }
    }
}
