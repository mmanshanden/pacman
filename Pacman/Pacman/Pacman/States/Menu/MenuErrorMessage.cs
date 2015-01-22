using Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class MenuErrorMessage : Menu
    {
        MenuGameMode mainmenu;
        string message;

        public MenuErrorMessage(string message)
        {
            base.controlSprite = "back";
            this.message = message;
        }

        public override IGameState TransitionTo()
        {
            if (this.mainmenu != null)
                return this.mainmenu;

            return this;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                this.mainmenu = new MenuGameMode();
        } 

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.DrawString(this.message, Vector2.One * 0.5f, DrawHelper.Origin.Center, Color.White);
            
            
            base.Draw(drawHelper);
        }
    }
}
