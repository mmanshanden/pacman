using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Network
{
    public class GameServer
    {
        private bool Debug = false;

        const float SendInterval =  1/ 32f;
        const int ServerPort = 1000;

        private Thread serverThread;
        private bool serverRunning;
        private bool serverStarted;
        private float timer;

        private NetServer server;
        private NetIncomingMessage inc;

        private NetMessage sendData;
        private List<NetMessage> receivedData; // fifo queue

        private int lastDistributedId;
        private Dictionary<NetConnection, Connection> connections;

        public struct Connection
        {
            public int Id;
            public string Ip;
        }

        public bool Visible
        {
            get;
            set;
        }

        public GameServer()
        {
            this.serverRunning = false;
            this.serverStarted = false;
            this.timer = SendInterval;            

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.ConnectionTimeout = 15;
            config.MaximumConnections = 1;
            config.Port = ServerPort;

            this.server = new NetServer(config);

            this.receivedData = new List<NetMessage>();

            this.lastDistributedId = 0;
            this.connections = new Dictionary<NetConnection, Connection>();
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

        public List<Connection> GetConnections()
        {
            return this.connections.Values.ToList();
        }

        /// <summary>
        /// Returns first, unread, received netmessage.
        /// </summary>
        /// <returns></returns>
        public NetMessage GetData()
        {
            if (this.receivedData.Count == 0)
                return null;

            NetMessage msg = this.receivedData[0];
            this.receivedData.Remove(msg);
            return msg;
        }

        /// <summary>
        /// Removes all received messages. Useful for clearing and
        /// outdated queue as a result of gamestate transitions.
        /// </summary>
        public void ClearData()
        {
            this.receivedData.Clear();
        }

        #region Threading
        /// <summary>
        /// Starts server in separate thread
        /// </summary>
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

        /// <summary>
        /// Stops server if running in separate thread
        /// </summary>
        public void Stop()
        {
            if (!this.serverStarted)
                return;

            this.server.Shutdown("server stopped");
            this.serverRunning = false;
        }

        /// <summary>
        /// Run thread
        /// </summary>
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

        /// <summary>
        /// Handle incoming messages of DiscoveryRequest type.
        /// </summary>
        private void ReceiveDiscoveryRequest()
        {
            if (!this.Visible)
                return;

            NetOutgoingMessage respone = this.server.CreateMessage();
            respone.Write("server");
            respone.Write(server.Connections.Count);

            server.SendDiscoveryResponse(respone, inc.SenderEndpoint);
        }

        /// <summary>
        /// Handle incoming messages of ConnectionApproval type.
        /// </summary>
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
            message.ConnectionId = this.lastDistributedId + 1;
            message.WriteMessage(outmsg);

            // add to list
            Connection connection = new Connection();
            connection.Id = message.ConnectionId;
            connection.Ip = inc.SenderEndpoint.Address.ToString();
            this.connections[inc.SenderConnection] = connection;

            // update id for new connections
            this.lastDistributedId++;
            
            // allow start up time
            Thread.Sleep(200);

            // send reply
            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

            if (Debug)
                Console.WriteLine("Login reply send back to " + inc.SenderEndpoint.Address.ToString());
        }

        /// <summary>
        /// Send message to all client when send interval elapses.
        /// </summary>
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
            this.server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

            // message has been send, don't send again
            this.sendData = null;

            if (Debug)
                Console.WriteLine("Message sent to connected clients");
        }

        /// <summary>
        /// Handles receiving messages.
        /// </summary>
        /// <param name="message">Received message</param>
        private void ReceiveMessage(NetMessage message)
        {
            this.receivedData.Add(message);
        }

        /// <summary>
        /// Updates the send interval timer, 
        /// receives data and
        /// send data when send timer elapses
        /// </summary>
        /// <param name="dt">Elapsed time in seconds</param>
        public void Update(float dt)
        {   
            // update timer, send msg
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendMessage();
                this.timer = SendInterval;
            }

            // receive message
            this.inc = server.ReadMessage();

            if (inc == null)
                return; // empty

            switch (inc.MessageType)
            {
                case NetIncomingMessageType.DiscoveryRequest:
                    this.ReceiveDiscoveryRequest();
                    break;

                case NetIncomingMessageType.ConnectionApproval:
                    this.ReceiveLogin();
                    break;

                case NetIncomingMessageType.StatusChanged:
                    // only interested in disconnect types
                    if (inc.SenderConnection.Status != NetConnectionStatus.Disconnected &&
                        inc.SenderConnection.Status != NetConnectionStatus.Disconnecting)
                        break;

                    // prepare message to lest host know who disconnected
                    NetMessage dcMessage = new NetMessage();
                    dcMessage.ConnectionId = this.connections[inc.SenderConnection].Id;
                    dcMessage.Type = PacketType.Logout;
                    this.receivedData.Add(dcMessage);

                    // remove from ids
                    this.connections.Remove(inc.SenderConnection);
                    break;

                case NetIncomingMessageType.Data:
                    NetMessage message = new NetMessage();
                    message.ReadMessage(inc); // parse
                    this.ReceiveMessage(message);
                    break;
            }

            this.server.Recycle(this.inc);
        }
    }
}
