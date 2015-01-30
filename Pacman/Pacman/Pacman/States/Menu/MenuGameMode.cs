﻿using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pacman
{
    class MenuGameMode : Menu
    {
        IGameState nextState;
        int selectedMenu;

        public MenuGameMode()
        {
            base.controlSprite = "gamemode";
        }        

        public override IGameState TransitionTo()
        {
            if (this.nextState != null)
                return this.nextState;

            else
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
                // Start a Singleplayer game
                if (this.selectedMenu == 0)
                {
                    this.nextState = new StatePlaying(1);
                }

                // Go to Multiplayer Server Selection
                if (this.selectedMenu == 1)
                {
                    this.nextState = new MenuServerBrowser(); 
                }
            }
        }

        public override void Update(float dt)
        {            
            // index must be positive
            selectedMenu = Math.Abs(selectedMenu);

            // index must be 0 or 1
            selectedMenu = selectedMenu % 2;
        }

        public override void Draw(DrawHelper drawHelper)
        {
            switch (this.selectedMenu)
            {
                case 0:
                    drawHelper.DrawBox("menu_gamemode_singleplayer", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
                case 1:
                    drawHelper.DrawBox("menu_gamemode_multiplayer", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center);
                    break;
            }
            
            base.Draw(drawHelper);
        }

    }
}
