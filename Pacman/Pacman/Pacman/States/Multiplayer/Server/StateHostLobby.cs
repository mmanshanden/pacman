using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;

namespace Pacman
{
    class StateHostLobby : IGameState
    {
        GameServer server;

        public enum GameModes
        {
            Multi,
            Player
        }

        public StateHostLobby()
        {
            this.server = new GameServer();
            this.server.StartSimple();
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

        }

        public void Draw(DrawHelper drawHelper)
        {

        }

    }
}
