using Base;
using Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using _3dgl;
using System.Collections.Generic;

namespace Pacman
{
    class MenuServerBrowser : Menu
    {
        DiscoveryClient client;
        IGameState nextState;

        private int serverIndex;
        private List<DiscoveryClient.DiscoveryReply> servers;

        public MenuServerBrowser()
        {
            base.controlSprite = "menu_controls_ps_serverbrowser";

            this.client = new DiscoveryClient();
            this.client.Discover();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.X))
                this.nextState = new StateHostLobby();

            if (inputHelper.KeyPressed(Keys.R))
                this.client.Discover();

            if (inputHelper.KeyPressed(Keys.Back))
                this.nextState = new MenuGameMode();

            if (inputHelper.KeyPressed(Keys.Up))
                this.serverIndex--;

            if (inputHelper.KeyPressed(Keys.Down))
                this.serverIndex++;

            if (inputHelper.KeyPressed(Keys.Enter))
            {
                if (this.servers.Count == 0)
                    return;

                string endpoint = this.servers[this.serverIndex].Endpoint;
                this.nextState = new StateJoinLobby(endpoint);
            }

        }

        public override IGameState TransitionTo()
        {
            if (nextState != null)
            {
                Console.Visible = false;
                return nextState;
            }
                
            return this;
        }

        public override void Update(float dt)
        {            
            if (this.nextState != null)
                return;

            this.client.Update();

            // positive index
            this.serverIndex = System.Math.Abs(this.serverIndex);

            // update server list
            this.servers = this.client.Replies;

            if (this.servers.Count == 0)
                return;

            // limit index
            this.serverIndex %= this.servers.Count;

        }

        public override void Draw(DrawHelper drawHelper)
        {
            Console.Clear();
            Console.Visible = true;

            for (int i = 0; i < this.servers.Count; i++)
            {
                DiscoveryClient.DiscoveryReply lobby = this.servers[i];

                string line = "";

                if (i == this.serverIndex)
                    line += "* ";

                line += lobby.Endpoint + " ";
                line += lobby.Connections + "/4";

                Console.WriteLine(line);
            }

            base.Draw(drawHelper);
        }

    }
}
