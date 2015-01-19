using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDefault : IGameState
    {
        public StateDefault()
        {

        }

        public void HandleInput(InputHelper inputHelper)
        {
            
        }

        public IGameState TransitionTo()
        {
            return new MenuMultiplayer();
        }

        public void Update(float dt)
        {

        }

        public void Draw(DrawHelper drawHelper)
        {

        }

    }
}
