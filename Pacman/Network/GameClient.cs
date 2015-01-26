using Lidgren.Network;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    /// <summary>
    /// Controls and maintains a NetClient form Lidgren framework.
    /// 
    /// Handles receiving and sending netmessages and logging in to 
    /// a gameserver.
    /// </summary>
    public class GameClient
    {
        // write debug messages to console
        private bool Debug = false;

        // interval in which to send messages to server.
        const float SendInterval = 1 / 32f;

        private int clientConnectionId;
        private int loginReplyRetries;
        private bool loginReplyReceived;
        private float timer; // used for send interval

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
            this.timer = SendInterval;

            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.ConnectionTimeout = 15;

            this.client = new NetClient(config);
            this.client.Start();

            this.Connected = true;
        }

        /// <summary>
        /// Tries to establish a connection with server at 
        /// provided endpoint.
        /// </summary>
        /// <param name="endpoint">A string containing the ip address</param>
        public void ConnectToServer(string endpoint)
        {
            NetOutgoingMessage msg = this.client.CreateMessage();
            msg.Write((byte)0);

            this.client.Connect(endpoint, 1000, msg);
        }

        /// <summary>
        /// This lets server know that client was disconnected.
        /// </summary>
        public void Disconnect()
        {
            this.client.Disconnect("leaving");
        }

        /// <summary>
        /// Returns the last received netmessage.
        /// </summary>
        /// <returns>Last received netmessage</returns>
        public NetMessage GetData()
        {
            NetMessage message = this.receivedData;
            this.receivedData = null;
            return message;
        }

        /// <summary>
        /// Updates the message that needs to be send when
        /// send interval elapses.
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SetData(NetMessage message)
        {
            this.sendData = message;
        }

        #region Login, receive and send
        /// <summary>
        /// Receive connection id from server,
        /// </summary>
        /// <param name="message">Message containing login reply</param>
        private void RecieveLogin(NetMessage message)
        {
            if (Debug)
                Console.WriteLine("Received login relpy.\n" + message.ToString());

            this.clientConnectionId = message.ConnectionId;
        }
        
        /// <summary>
        /// Handles received messages.
        /// </summary>
        /// <param name="message">Received message</param>
        private void ReceiveMessage(NetMessage message)
        {
            this.receivedData = message;
        }

        /// <summary>
        /// Handles sending messages when send interval elapses.
        /// </summary>
        private void SendMessage()
        {
            if (this.sendData == null)
                return;

            NetOutgoingMessage msg = this.client.CreateMessage();
            this.sendData.WriteMessage(msg);

            // UnrealiableSequenced because losing messages is not a deal
            // and it reduces the overhead by some amount.
            this.client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
            
            if (Debug)
            {
                Console.WriteLine("Message sent to server: ");
                Console.WriteLine(sendData.ToString());
            }

            // data sent, don't send again.
            this.sendData = null;
        }

        /// <summary>
        /// Returns message content object. The id field is set to
        /// clients connection id.
        /// </summary>
        /// <returns>Message content object</returns>
        public NetMessageContent ConstructContentMessage()
        {
            NetMessageContent cmsg = new NetMessageContent();
            cmsg.Id = this.clientConnectionId;

            return cmsg;
        }

        /// <summary>
        /// Updates the send interval timer, 
        /// receives data and
        /// send data when send timer elapses
        /// </summary>
        /// <param name="dt">Elapsed time in seconds</param>
        public void Update(float dt)
        {
            this.timer -= dt;
            if (this.timer < 0)
            {
                this.SendMessage();
                this.timer = SendInterval;
            }

            // login timeout
            if (!this.loginReplyReceived)
            {
                this.loginReplyRetries++;

                if (this.loginReplyRetries > 100)
                    this.Connected = false;
            }
            
            // read msg
            this.inc = this.client.ReadMessage();

            if (inc == null)
                return;


            // dc check
            if (inc.MessageType == NetIncomingMessageType.StatusChanged)
            {
                NetConnectionStatus status = (NetConnectionStatus)inc.ReadByte();
                this.Connected = (status != NetConnectionStatus.Disconnected);
                return;
            }

            if (inc.MessageType != NetIncomingMessageType.Data)
                return;


            // parse message
            NetMessage msg = new NetMessage();
            msg.ReadMessage(inc);


            if (msg.Type == PacketType.Login)
            {
                this.RecieveLogin(msg);
                this.loginReplyReceived = true;
            }

            // handle message
            this.ReceiveMessage(msg);
            
            // use same inc object
            this.client.Recycle(this.inc);
        }
        #endregion
    }
}
