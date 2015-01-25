using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Pacman
{
    class StatePlaying : IGameState
    {
        FileReader levelFile;
        Level level;
        IGameState nextState, nextLevel;

        int index;

        public StatePlaying(int index, int score = 0)
        {
            this.index = index;

            // load level and game objects.
            string path = "Content/Levels/singleplayer/level" + index + ".txt";

            this.levelFile = new FileReader(path);
            this.level = new Level();

            this.level.LoadGameBoard(levelFile.ReadGrid("level"));
            this.level.LoadGameBoardObjects(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Spawn = this.levelFile.ReadVector("player_position");
            player.Score = score;
            this.level.Add(player);

            GhostHouse ghostHouse = new GhostHouse();
            ghostHouse.Entry = this.levelFile.ReadVector("ghosthouse_entry");
            ghostHouse.Center = this.levelFile.ReadVector("ghosthouse_center");
            ghostHouse.SetPacman(player);
            this.level.Add(ghostHouse);

            Blinky blinky = Blinky.LoadBlinky(levelFile);
            Pinky pinky = Pinky.LoadPinky(levelFile);
            Inky inky = Inky.LoadInky(levelFile);
            Clyde clyde = Clyde.LoadClyde(levelFile);

            ghostHouse.Add(blinky);
            ghostHouse.Add(clyde);
            ghostHouse.Add(inky);
            ghostHouse.Add(pinky);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Escape))
                this.nextState = new StatePaused(this);

            this.level.HandleInput(inputHelper);
        }

        public IGameState TransitionTo()
        {
            // Go to next level if it has been set
            if (nextLevel != null)
                return nextLevel; 

            // If lives are 0 we go to game over state
            if (this.level.Player.Lives < 1)
                return new StateGameOver(this.level.Player);

            if (nextState != null)
            {
                // set next state to null such that returning
                // to this state won't result into an immediate
                // transition back into pause state.
                IGameState paused = nextState;
                nextState = null;
                return paused;
            }

            return this;
        }

        public void Update(float dt)
        {
            this.level.Update(dt);

            if (this.level.GetBubbles().Count == 0)
            {
                if (this.index != 2) // Go to next level
                    this.nextLevel = new StatePlaying(index + 1, this.level.Player.Score);
                else
                    this.nextLevel = new StateGameOver(this.level.Player);
                    // Go to GameOver state and handle Victory there.
            }
                
        }

        public void Draw(DrawManager drawManager)
        {
            this.level.Draw(drawManager);
        }

        public void Draw(DrawHelper drawHelper)
        {
            this.level.Draw(drawHelper);
        }

    }
}
