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
        NetServer server;
        NetPeerConfiguration config;
        List<NetPlayer> players;

        NetIncomingMessage inc;
        DateTime time;
        // Create timespan of 30ms
        TimeSpan timetopass = new TimeSpan(0, 0, 0, 3, 30);

        Thread serverThread;

        bool started = false;
        bool running = true;

        public GameServer()
        {
            config = new NetPeerConfiguration("game");
            config.Port = 1000;
            config.MaximumConnections = 100;
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

            server = new NetServer(config);

            server.Start();

            Console.WriteLine("SERVER | server started");

            this.players = new List<NetPlayer>();

            time = DateTime.Now;
        }

        #region Threading
        public void Start()
        {
            if (this.started)
                return;

            this.started = true;
            this.serverThread = new Thread(new ThreadStart(Run));
            this.serverThread.IsBackground = true;
            this.serverThread.Start();
        }

        private void Run()
        {
            while (running)
                Update();
        }
        #endregion


        private void StatusChanged(NetIncomingMessage inc)
        {
            Console.WriteLine("SERVER | " + inc.SenderConnection.ToString() + " status changed. " + (NetConnectionStatus)inc.SenderConnection.Status);
            if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected ||
                inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
            {
                // Find disconnected character and set it's connected status to false!
                foreach (NetPlayer netPlayer in players)
                {
                    if (netPlayer.Connection == inc.SenderConnection)
                    {
                        if (!netPlayer.IsHost) // For Testing purposes this is currently set to the client that is not Host! ###
                            running = false;
                        netPlayer.Connected = false;
                        Console.WriteLine("SERVER | " + netPlayer.ID + " just went away, so set his status to Disconnected!");
                        break;
                    }
                }
            }
        }

        #region NetRoutine
        private void ReceiveLogin(NetIncomingMessage inc)
        {
            Console.WriteLine("SERVER | New connection incoming...");

            // Approve connection (critical)
            inc.SenderConnection.Approve();

            // add player to list
            NetPlayer player = new NetPlayer();
            player.ID = inc.SenderConnection.GetHashCode();
            player.IsHost = (this.players.Count == 0);
            this.players.Add(player);

            // debug
            Console.WriteLine("SERVER | New player connected: \n" + player.ToString());

            // reply
            NetOutgoingMessage outmsg = server.CreateMessage();
            outmsg.Write((byte)PacketType.Login);
            outmsg.Write(player.ID);
            outmsg.Write(player.IsHost);

            // allow start-up time
            Thread.Sleep(200);

            server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);
            Console.WriteLine("SERVER | Replied with id hash");

        }


        private void ReceivePacket(PacketType type)
        {
            switch (type)
            {
                case PacketType.WorldState:
                    int incomingId = inc.SenderConnection.GetHashCode();
                    NetPlayer np = MessageParser.ReadNetPlayerList(inc)[0];

                    foreach (NetPlayer player in this.players)
                    {
                        if (player.ID != incomingId)
                            continue;

                        player.Position = np.Position;
                        player.Direction = np.Direction;
                        player.Speed = np.Speed;
                        player.Time = np.Time;

                        Console.WriteLine("SERVER | Update player: \n" + player.ToString());
                    }
                    break;
            }
        }

        private void SendWorldState()
        {
            int connections = server.ConnectionsCount;

            Console.WriteLine("SERVER | # Of connections: " + connections);

            if (connections == 0)
                return;

            NetOutgoingMessage msg = server.CreateMessage();
            MessageParser.WritePacketType(msg, PacketType.WorldState);
            MessageParser.WriteNetPlayerList(msg, this.players);

            server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

            Console.WriteLine("SERVER | Sent a gameworld update to all clients " + DateTime.Now);
        }

        public void Update()
        {
            inc = server.ReadMessage();

            // gameworld update every *30* milliseconds 
            if ((time + timetopass) < DateTime.Now)
            {
                SendWorldState();
                time = DateTime.Now;
            }

            if (inc == null)
                return;

            switch (inc.MessageType)
            {
                case NetIncomingMessageType.ConnectionApproval:
                    if (inc.ReadByte() == (byte)PacketType.Login)
                    {
                        ReceiveLogin(inc);
                    }

                    break;

                case NetIncomingMessageType.Data:
                    PacketType datamessage = (PacketType)inc.ReadByte();
                    this.ReceivePacket(datamessage);
                    break;


                case NetIncomingMessageType.StatusChanged:
                    StatusChanged(inc);
                    break;

                //discovery request ###

                default:
                    // in case there is another kind of message 
                    Console.WriteLine("SERVER | Received a status update message but didn't have anything for it.");
                    break;
            }

        }
        #endregion
    }
}
