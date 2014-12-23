using Microsoft.Xna.Framework;

namespace Base
{
    public interface IGameState : ILoopMember
    {
        IGameState TransitionTo();
        void HandleInput(InputHelper inputHelper);
    }
}
