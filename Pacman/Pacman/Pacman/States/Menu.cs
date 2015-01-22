using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
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

        public virtual void HandleInput(InputHelper inputHelper) { }
        public virtual void Update(float dt) { }
        public virtual void Draw(DrawManager drawManager) { }
    }
}
