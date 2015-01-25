using Microsoft.Xna.Framework;

namespace Base
{
    public interface IGameState : ILoopMember
    {
        /// <summary>
        /// Returns the new GameState, or current (this) if
        /// no transition is meant to happen
        /// </summary>
        IGameState TransitionTo();
        /// <summary>
        /// Update gameObject with keyboard/gamepad input.
        /// </summary>
        void HandleInput(InputHelper inputHelper);
    }
}
