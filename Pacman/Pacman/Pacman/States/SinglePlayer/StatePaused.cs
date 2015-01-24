using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pacman
{
    class StatePaused : Menu
    {
        IGameState nextState;
        IGameState savedState;
        int selectedMenu;
        bool includeUpdate;

        public StatePaused(IGameState gameState, bool includeUpdate = false)
        {
            base.controlSprite = "gamemode"; 
            this.savedState = gameState;
            this.includeUpdate = includeUpdate;
        }

        public override IGameState TransitionTo()
        {
            if (nextState != null)
                return nextState; 

            return this;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Up))
                selectedMenu--;

            if (inputHelper.KeyPressed(Keys.Down))
                selectedMenu++;

            if (inputHelper.KeyPressed(Keys.Enter))
            {
                // Return to game
                if (this.selectedMenu == 0)
                {
                    this.nextState = savedState;
                }
                // Return to main menu
                if (this.selectedMenu == 1)
                {
                    this.nextState = new MenuGameMode(); 
                }
            }
            // Return to game
            if (inputHelper.KeyPressed(Keys.Escape))
                nextState = savedState;

            // Return to main menu
            if (inputHelper.KeyPressed(Keys.Back))
                nextState = new MenuGameMode(); 
        }

        public override void Update(float dt)
        {
            // index must be positive
            selectedMenu = Math.Abs(selectedMenu);

            // index must be 0 or 1
            selectedMenu = selectedMenu % 2;

            if (this.includeUpdate)
                this.savedState.Update(dt);

            base.Update(dt);
        }

        public override void Draw(DrawManager drawManager)
        {
            this.savedState.Draw(drawManager); 
        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.DrawBox("PauseOverlay", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);

            switch (this.selectedMenu)
            {
                case 1:
                    drawHelper.DrawBox("menu_gamemode_singleplayer", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
                case 0:
                    drawHelper.DrawBox("menu_gamemode_multiplayer", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
            }

            base.Draw(drawHelper);
        }

    }
}
