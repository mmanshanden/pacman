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
        }


        public void StartSimple()
        {
            this.server.Start();
            this.serverStarted = true;
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
            respone.Write("");
            respone.Write(server.Connections.Count);

            server.SendDiscoveryResponse(respone, inc.SenderEndpoint);
        }

        private void ReceiveLogin()
        {
            // Approve connection (critical)
            inc.SenderConnection.Approve();

            // reply
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write("test");

            // send reply
            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
        }

        private void ReceiveMessage()
        {

        }

        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
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
                    this.ReceiveMessage();
                    break;
            }

            if (inc.MessageType == NetIncomingMessageType.ConnectionApproval)
                this.ReceiveLogin();

            if (inc.MessageType != NetIncomingMessageType.Data)
                return;

            // do stuff with message
        }
    }
}
