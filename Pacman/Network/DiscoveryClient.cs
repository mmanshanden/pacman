using Lidgren.Network;
using System.Collections.Generic;

namespace Network
{
    /// <summary>
    /// A small network client only capable of
    /// sending discovery requests and receiving
    /// discovery replies.
    /// </summary>
    public class DiscoveryClient
    {
        const int ServerPort = 1000;

        private NetClient client;
                
        public struct DiscoveryReply
        {
            public string Name;
            public string Endpoint;
            public int Connections;
        }

        public List<DiscoveryReply> Replies = new List<DiscoveryReply>();

        public DiscoveryClient()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("game");
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);

            this.client = new NetClient(config);
            this.client.Start();
        }

        public void Discover()
        {
            this.Replies.Clear();
            
            // LAN
            this.client.DiscoverLocalPeers(ServerPort);

            // vpn (for testing)
            this.client.DiscoverKnownPeer("10.0.0.1", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.2", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.3", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.4", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.5", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.6", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.7", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.8", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.9", ServerPort);
            this.client.DiscoverKnownPeer("10.0.0.10", ServerPort);
        }

        /// <summary>
        /// Read discovery replies.
        /// </summary>
        public void Update()
        {
            NetIncomingMessage inc = this.client.ReadMessage();

            if (inc == null || inc.MessageType != NetIncomingMessageType.DiscoveryResponse)
                return;

            DiscoveryReply reply = new DiscoveryReply();
            reply.Name = inc.ReadString();
            reply.Endpoint = inc.SenderEndpoint.Address.ToString();
            reply.Connections = inc.ReadInt32();

            this.Replies.Add(reply);
        }

    }
}
