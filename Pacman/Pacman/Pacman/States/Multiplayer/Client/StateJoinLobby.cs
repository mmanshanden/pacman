using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateJoinLobby : Menu
    {
        GameClient client;
        LobbyMessage lobbyState;
        IGameState nextState;
        StateJoinGame game;

        public StateJoinLobby(string endpoint)
        {
            base.controlSprite = "back";

            this.client = new GameClient();
            this.client.ConnectToServer(endpoint);

            Console.Clear();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Keys.Back))
            {
                this.client.Disconnect();
                this.nextState = new MenuServerBrowser(); 
            }
        }

        public override IGameState TransitionTo()
        {
            if (this.game != null)
                return this.game;

            if (this.nextState != null)
                return this.nextState; 

            return this;
        }

        public override void Update(float dt)
        {
            this.client.Update(dt);

            NetMessage received = this.client.GetData();

            if (received == null)
                return;

            switch (received.Type)
            {
                case PacketType.Lobby:
                    this.lobbyState = received.GetData() as LobbyMessage;
                    break;
                case PacketType.WorldState:
                    this.game = new StateJoinGame(client);
                    break;                    
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;

            if (this.lobbyState != null)
            {
                Console.Clear();
                Console.WriteLine("Players in lobby: " + this.lobbyState.PlayerCount);
            }
            else
            {
                Console.WriteLine("We don't have lobby!");
            }

            base.Draw(drawHelper);
        }

    }
}
