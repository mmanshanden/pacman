using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateJoin : IGameState
    {
        GameClient client;
        Level level;

        public StateJoin(string endpoint)
        {
            this.client = new GameClient();
            this.client.ConnectToServer(endpoint);

            Console.Clear();
            Console.WriteLine("Joining server " + endpoint);

            this.level = new Level();
            this.level.LoadLevel("Content/level1.txt");
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.level.HandleInput(inputHelper);
        }

        public IGameState TransitionTo()
        {
            return this;
        }

        public void Update(float dt)
        {
            this.level.Update(dt);

            this.client.Update(dt);

            // lets send some data to the server
            NetMessage send = new NetMessage();
            send.PacketType = PacketType.WorldState;
            send.DataType = DataType.Playing;
            // we don't have to worry about connection id or time
            // time is added by the client and the server acquires
            // our connection id by itself.

            // now we tell te client to send this data
            // this will not be sent immediately however.
            // the client will send the message once the
            // sendtime is over.
            this.client.SetData(send);

            // lets see if the server sent us some data
            NetMessage received = this.client.GetData();

            if (received == null)
                return; // no data. we can stop here

            // lets see what type of data the server sent us
            switch(received.DataType)
            {
                case DataType.Playing:
                    // We now know the NetMessage was originally 
                    // a playingmessage. So we can convert it to
                    // a playingmessage;
                    PlayingMessage message = new PlayingMessage();
                    NetMessage.CopyOver(received, message);

                    // this is the data we received from the server:
                    Console.WriteLine("Message received from server:");
                    Console.WriteLine(message.ToString());
                    break;
            }
            

        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(14, 14);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 14f, 1 / 14f);
        }

    }
}
