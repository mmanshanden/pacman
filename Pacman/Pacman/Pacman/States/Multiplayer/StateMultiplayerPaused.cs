using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Network;
using System;

namespace Pacman
{
    class StateMultiplayerPaused : Menu
    {
        IGameState nextState;
        IGameState savedState;
        int selectedMenu;
        bool includeUpdate;

        GameServer server;
        GameClient client; 

        public StateMultiplayerPaused(IGameState gameState, GameServer server, bool includeUpdate = false)
        {
            base.controlSprite = "gamemode";
            this.savedState = gameState;
            this.includeUpdate = includeUpdate;
            this.server = server; 
        }

        public StateMultiplayerPaused(IGameState gameState, GameClient client, bool includeUpdate = false)
        {
            base.controlSprite = "gamemode";
            this.savedState = gameState;
            this.includeUpdate = includeUpdate;
            this.client = client; 
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
                    if (this.server != null)
                        server.Stop();
                    if (this.client != null)
                        client.Disconnect(); 

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
            drawHelper.DrawOverlay(Color.Black * 0.8f, Vector2.Zero, Vector2.One);

            switch (this.selectedMenu)
            {
                case 0:
                    drawHelper.DrawBox("menu_pause_resume", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
                case 1:
                    drawHelper.DrawBox("menu_pause_mainmenu", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
            }

            base.Draw(drawHelper);
        }

    }
}
