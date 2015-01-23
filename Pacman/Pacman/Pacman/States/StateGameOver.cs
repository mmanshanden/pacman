using Base;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework; 

namespace Pacman
{
    class StateGameOver : Menu
    {
        List<Pacman> players;

        IGameState nextState;
        private bool playGameOverSound;
        private bool Victory;
        private int total_lives = 0;
        private int total_Score = 0; 

        public StateGameOver(Pacman pacman)
        {
            base.controlSprite = "back";

            this.players = new List<Pacman>();
            this.players.Add(pacman);

            if (pacman.Lives > 0)
                this.Victory = true;
            else
                this.playGameOverSound = true;

            this.total_Score = pacman.Score; 
        }

        public StateGameOver(List<Pacman> players)
        {
            base.controlSprite = "back";

            this.players = players.OrderByDescending(o => o.Score).ToList(); 

            foreach (Player player in players)
                this.total_lives += player.Lives;

            if (this.total_lives > 0)
                this.Victory = true;
            else
                this.playGameOverSound = true; 
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

            // Adjust to nice looking stuff

            if (Victory)
                drawHelper.DrawStringBig("Victory!", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center, Color.White);
            else
                drawHelper.DrawStringBig("Game over!", new Vector2(0.5f, 0.5f), DrawHelper.Origin.Center, Color.White);


            for (int i = 0; i < players.Count; i++)
            {
                if (players.Count == 1)
                    drawHelper.DrawString("You scored: " + this.total_Score, new Vector2(0.1f, 0.1f), DrawHelper.Origin.Center, Color.White);
                else
                {
                    drawHelper.DrawString("Player " + (i + 1).ToString() + " scored: " + players[i].Score.ToString(), new Vector2(0.5f, 0.1f + 0.1f * i), DrawHelper.Origin.Center, Color.White);
                    drawHelper.DrawString("Total score: " + this.total_Score.ToString(), new Vector2(0.5f, 0.1f + 0.1f * (i + 1)), DrawHelper.Origin.Center, Color.White);
                }
            }
        }
    }
}
