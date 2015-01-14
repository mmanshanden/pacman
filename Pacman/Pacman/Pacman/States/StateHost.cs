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

            PlayingMessage message = new PlayingMessage();
            PlayingMessage.Player player = new PlayingMessage.Player();
            player.Position = new Vector2(10, 12);
            message.Players.Add(player);
            message.Players.Add(player);
            this.server.SetData(message);
        }

        public void Draw(DrawHelper drawHelper)
        {
            
        }

    }
}
