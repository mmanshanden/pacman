﻿using _3dgl;
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

        public StatePlaying(int indeX)
        {
            if (indeX > 2) // put max count here to prevent level not found :/
                indeX = 2;
            this.index = indeX;
            string path = "Content/Levels/singleplayer/level" + index.ToString() + ".txt";

            this.levelFile = new FileReader(path);
            this.level = new Level();

            this.level.LoadGameBoard(levelFile.ReadGrid("level"));
            this.level.LoadGameBoardObjects(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Spawn = this.levelFile.ReadVector("player_position");
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
            if (nextLevel != null)
                return nextLevel; 

            if (this.level.Player.Lives < 1 || this.level.GetBubbles().Count == 0)
                return new StateGameOver(this.level.Player);

            if (nextState != null)
            {
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
                this.nextLevel = new StatePlaying(this.index + 1); 
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
