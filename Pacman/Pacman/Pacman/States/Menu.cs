using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    /// <summary>
    /// Basic menu class. Shows a control sprite
    /// in bottom right corner of the screen.
    /// </summary>
    class Menu : IGameState
    {
        protected string controlSprite;

        public Menu()
        {

        }
        
        public virtual IGameState TransitionTo()
        {
            return this;
        }

        public virtual void Draw(DrawHelper drawHelper)
        {
            if (this.controlSprite == "")
                return;

            string platform = InputHelper.ControllerConnected ? "ps_" : "kb_";
            string controls = "menu_controls_" + platform + controlSprite;

            drawHelper.DrawBox(controls, Vector2.One * 0.95f, DrawHelper.Origin.BottomRight);
        }

        public virtual void Update(float dt)
        {
            Game.SoundManager.PlaySong("");
        }

        public virtual void HandleInput(InputHelper inputHelper) { }
        public virtual void Draw(DrawManager drawManager) { }
    }
}
