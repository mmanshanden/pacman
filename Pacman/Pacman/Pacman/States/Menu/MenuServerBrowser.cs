using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class MenuServerBrowser : IGameState
    {
        DiscoveryClient client;
        IGameState nextState;

        public MenuServerBrowser()
        {
            this.client = new DiscoveryClient();
            this.client.Discover();
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.X))
                this.nextState = new StateHostGame();

            if (inputHelper.KeyPressed(Keys.R))
                this.client.Discover();

            int number = -1;

            if (inputHelper.KeyPressed(Keys.D1))
                number = 0;
            if (inputHelper.KeyPressed(Keys.D2))
                number = 1;
            if (inputHelper.KeyPressed(Keys.D3))
                number = 2;

            if (number == -1)
                return;

            if (number > client.Replies.Count)
                return;

            this.nextState = new StateJoinGame(this.client.Replies[number].Endpoint);

        }

        public IGameState TransitionTo()
        {
            if (nextState != null)
            {
                Console.Visible = false;
                return nextState;
            }
                
            return this;
        }

        public void Update(float dt)
        {            
            if (this.nextState != null)
                return;

            this.client.Update();
            Console.Clear();

            foreach(DiscoveryClient.DiscoveryReply server in this.client.Replies)
            {
                string line = "";
                line += server.Endpoint + ": ";
                line += server.Name + ". ";
                line += server.Connections + " connections.";

                Console.WriteLine(line);
            }

            Console.WriteLine("\n\nPress r to refresh list.");
            Console.WriteLine("Press x to host server.");
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
        }

    }
}
