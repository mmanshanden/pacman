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
        private bool Debug = false;

        const float UpdateTimer =  1/ 32f;
        const int ServerPort = 1000;

        private Thread serverThread;
        private bool serverRunning;
        private bool serverStarted;
        private float timer;

        private NetServer server;
        private NetIncomingMessage inc;

        private NetMessage sendData;
        private List<NetMessage> receivedData;
        private List<int> connectedIds;

        public bool Visible
        {
            get;
            set;
        }

        public GameServer()
        {
            this.serverRunning = false;
            this.serverStarted = false;
            this.timer = UpdateTimer;            

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.ConnectionTimeout = 15;
            config.MaximumConnections = 1;
            config.Port = ServerPort;

            this.server = new NetServer(config);

            this.receivedData = new List<NetMessage>();
            this.connectedIds = new List<int>();
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
        public void ClearData()
        {
            this.receivedData.Clear();
        }

        public List<string> GetConnectedIPs()
        {
            List<string> result = new List<string>();

            foreach(NetConnection connection in this.server.Connections)
                result.Add(connection.RemoteEndpoint.Address.ToString());

            return result;
        }

        public List<int> GetConnectedIDs()
        {
            return this.connectedIds;
        }

        #region Threading
        public void Start()
        {
            if (this.serverStarted)
                throw new Exception("Cannot start server more than once.");

            this.server.Start();
            this.serverRunning = true;
            this.serverStarted = true;

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
            if (!this.Visible)
                return;

            NetOutgoingMessage respone = this.server.CreateMessage();
            respone.Write("server");
            respone.Write(server.Connections.Count);

            server.SendDiscoveryResponse(respone, inc.SenderEndpoint);
        }

        private void ReceiveLogin()
        {
            if (!this.Visible)
                return; // closed for business

            // Approve connection (critical)
            inc.SenderConnection.Approve();

            // reply
            NetOutgoingMessage outmsg = server.CreateMessage();
            NetMessage message = new NetMessage();
            message.Type = PacketType.Login;
            message.ConnectionId = inc.SenderConnection.GetHashCode();
            message.WriteMessage(outmsg);

            // add to list
            this.connectedIds.Add(message.ConnectionId);
            
            // allow start up time
            Thread.Sleep(200);

            // send reply
            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            if (Debug)
                Console.WriteLine("Login reply send back to " + inc.SenderEndpoint.Address.ToString());
        }

        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            if (this.server.Connections.Count == 0)
                return;
            
            // message
            NetOutgoingMessage msg = this.server.CreateMessage();
            this.sendData.WriteMessage(msg);

            // send
            this.server.SendToAll(msg, NetDeliveryMethod.ReliableSequenced);

            // message has been send, dont send again
            this.sendData = null;

            if (Debug)
                Console.WriteLine("Message sent to connected clients");
        }

        private void ReceiveMessage(NetMessage message)
        {
            message.ConnectionId = inc.SenderConnection.GetHashCode();
            this.receivedData.Add(message);
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

                case NetIncomingMessageType.StatusChanged:
                    if (inc.SenderConnection.Status != NetConnectionStatus.Disconnected &&
                        inc.SenderConnection.Status != NetConnectionStatus.Disconnecting)
                        break;

                    NetMessage dcMessage = new NetMessage();
                    dcMessage.ConnectionId = inc.SenderConnection.GetHashCode();
                    dcMessage.Type = PacketType.Logout;

                    this.connectedIds.Remove(dcMessage.ConnectionId);

                    this.receivedData.Add(dcMessage);
                    break;

                case NetIncomingMessageType.Data:
                    NetMessage message = new NetMessage();
                    message.ReadMessage(inc);
                    this.ReceiveMessage(message);
                    break;
            }

            this.server.Recycle(this.inc);
            this.Visible = false;
        }
    }
}
