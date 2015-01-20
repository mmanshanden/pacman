using Base;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pacman
{
    class MenuGameMode : IGameState
    {
        IGameState nextState;
        int selectedMenu;

        public MenuGameMode()
        {

        }        

        public IGameState TransitionTo()
        {
            if (this.nextState != null)
            {
                Console.Clear();
                Console.Visible = false;

                return this.nextState;
            }
            else
            {
                return this;
            }
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Up))
                selectedMenu--;

            if (inputHelper.KeyPressed(Keys.Down))
                selectedMenu++;

            if (inputHelper.KeyPressed(Keys.Enter))
            {
                if (this.selectedMenu == 0)
                {
                    this.nextState = new StatePlaying();
                }

                if (this.selectedMenu == 1)
                {
                    this.nextState = new MenuServerBrowser(); 
                }
            }
        }
        
        public void Update(float dt)
        {            
            // index must be positive
            selectedMenu = Math.Abs(selectedMenu);

            // index must be 0 or 1
            selectedMenu = selectedMenu % 2;
        }

        public void Draw(DrawHelper drawHelper)
        {
            Console.Visible = true;
            Console.Clear();

            string singleplayer = "Singleplayer";
            string multiplayer = "Multiplayer";

            // mark selected menu
            switch (this.selectedMenu)
            {
                case 0:
                    singleplayer = "* " + singleplayer;
                    break;
                case 1:
                    multiplayer = "* " + multiplayer;
                    break;
            }

            Console.WriteLine(singleplayer);
            Console.WriteLine(multiplayer);

        }
    }
}
