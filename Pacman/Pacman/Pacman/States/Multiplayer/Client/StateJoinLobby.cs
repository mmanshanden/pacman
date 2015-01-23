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
        public StateJoinLobby(GameClient client)
        {
            base.controlSprite = "back";

            this.client = client;
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

            if (!this.client.Connected)
                return new MenuErrorMessage("Could not connect to server.");

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
                    this.game = new StateJoinGame(client, received);
                    break;                    
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.lobbyState != null)
                drawHelper.DrawString("Players in lobby: " + this.lobbyState.PlayerCount, Vector2.One * 0.1f, DrawHelper.Origin.TopLeft, Color.White);
            else
                drawHelper.DrawString("Connecting to server...", Vector2.One * 0.1f, DrawHelper.Origin.TopLeft, Color.White);

            base.Draw(drawHelper);
        }

    }
}
