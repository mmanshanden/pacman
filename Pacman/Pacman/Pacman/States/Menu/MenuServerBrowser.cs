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
            base.controlSprite = "serverbrowser";

            this.client = new DiscoveryClient();
            this.client.Discover();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.X))
                this.nextState = new StateHostLobby(1);

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
                return nextState;

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
            for (int i = 0; i < MathHelper.Min(this.servers.Count, 6); i++)
            {
                DiscoveryClient.DiscoveryReply lobby = this.servers[i];

                string line = "";
                Color color = Color.White; 


                line += lobby.Endpoint + " ";
                line += " | Players: ";
                line += lobby.Connections + 1 + "/2";

                Vector2 position = new Vector2(0.5f, 0f); 
                position.Y += 0.1f * i;


                if (i == this.serverIndex)
                {
                    drawHelper.DrawOverlay(Color.White, new Vector2(0, position.Y), new Vector2(1f, 0.1f)); 
                    color = Color.Black;
                }

                drawHelper.DrawString(line, new Vector2(position.X, position.Y + 0.05f), DrawHelper.Origin.Center, color);
            }

            base.Draw(drawHelper);
        }

    }
}
