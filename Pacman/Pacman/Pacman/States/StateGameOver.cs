using Base;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    class StateGameOver : Menu
    {
        List<Pacman> players;

        IGameState nextState;
        private bool playGameOverSound;

        public StateGameOver(Pacman pacman)
        {
            base.controlSprite = "back";

            this.players = new List<Pacman>();
            this.players.Add(pacman);

            if (pacman.Lives == 0)
                this.playGameOverSound = true;
        }

        public StateGameOver(List<Pacman> players)
        {
            base.controlSprite = "back";
            
            this.players = players;
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

        public override void Update(float dt)
        {
            if (this.playGameOverSound)
            {
                //Game.SoundManager.PlaySoundEffect("game_over");
                this.playGameOverSound = false;
            }
                
            
            base.Update(dt);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);
        }
    }
}
