using Base;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class StateGameOver : Menu
    {
        Pacman player;

        IGameState nextState;
        private bool victory;

        public StateGameOver(Pacman pacman)
        {
            base.controlSprite = "back";

            this.player = pacman;
            this.victory = (pacman.Lives > 0);
        }

        public override IGameState TransitionTo()
        {
            if (this.nextState != null)
                return this.nextState;

            return this;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Back))
                this.nextState = new MenuGameMode();
        }
        
        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);

            if (victory)
                drawHelper.DrawStringBig("Victory!", new Vector2(0.5f, 0.2f), DrawHelper.Origin.Center, Color.White);
            else
                drawHelper.DrawStringBig("Game over!", new Vector2(0.5f, 0.2f), DrawHelper.Origin.Center, Color.White);

            drawHelper.DrawString("You scored: " + this.player.Score.ToString(), new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center, Color.White);
        }
    }
}

