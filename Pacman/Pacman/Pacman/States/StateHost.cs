using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateHost : IGameState
    {
        GameServer server;

        public StateHost()
        {
            this.server = new GameServer();
            this.server.StartSimple();

            Console.Clear();
            Console.WriteLine("Hosting server");
        }

        public void HandleInput(InputHelper inputHelper)
        {

        }

        public IGameState TransitionTo()
        {
            return this;
        }

        public void Update(float dt)
        {
            this.server.Update(dt);

            // prepare data for clients
            PlayingMessage send = new PlayingMessage();
            
            // begin arbitrary data
            PlayingMessage.Player player = new PlayingMessage.Player();
            player.Position = new Vector2(0.02134f, 12.12403f);
            player.Speed = 9001;
            send.Players.Add(player);

            PlayingMessage.Ghost ghost = new PlayingMessage.Ghost();
            ghost.Direction = new Vector2(1, 0);
            ghost.Target = new Vector2(6, 9);
            send.Ghosts.Add(ghost);
            // read all messages from server

            // tell server to send it when its timer elapses
            this.server.SetData(send);

            
            // get data that was sent back to server
            // i.e. the data the server received;
            NetMessage received;

            // pull messages until theres none left
            while((received = this.server.GetData()) != null) 
            {
                // this is the message that was received by the server;
                Console.WriteLine("Message received from client:");
                Console.WriteLine(received.ToString());
                Console.WriteLine("");
            }
        }

        public void Draw(DrawHelper drawHelper)
        {
            
        }

    }
}
