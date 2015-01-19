﻿using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        public Player Player { get; private set; }
        public GhostHouse GhostHouse { get; private set; }

        public Level()
        {

        }

        public List<Vector2> GetBubbles()
        {
            List<Vector2> result = new List<Vector2>();

            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObject is Bubble)
                    result.Add(gameObject.Position);
            }

            return result;
        }

        public List<Vector2> GetPowerUps()
        {
            List<Vector2> result = new List<Vector2>();

            foreach (GameObject gameObject in this.gameObjects)
            {
                if (gameObject is Powerup)
                    result.Add(gameObject.Position);
            }

            return result;
        }

        public void Add(Player player)
        {
            this.Player = player;
            base.Add(player);            
        }

        public void Add(GhostHouse ghostHouse)
        {
            this.GhostHouse = ghostHouse;
            base.Add(GhostHouse);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.Player.Direction = inputHelper.GetDirectionalInput();
        }

        public void LoadGameBoard(char[,] grid)
        {
            // initialize gameboard and add board to level
            GameBoard gameBoard = GameBoard.CopyDimensions(grid);

            for (int x = 0; x < gameBoard.Size.X; x++)
            {
                for (int y = 0; y < gameBoard.Size.Y; y++)
                {
                    // standard ground tile
                    GameTile tile = new Ground();

                    switch (grid[x, y])
                    {
                        case '#':
                            tile = new Wall();
                            break;
                        case 'o':
                            tile = new InvisibleWall();
                            break;
                        default:
                            tile = new Ground();
                            break;
                    }

                    gameBoard.Add(tile, x, y);
                }
            }

            this.Add(gameBoard);

            Vector2 target = this.GameBoard.Size / 2;
            Game.DrawManager.Camera.Target = new Vector3(target.X, 0, target.Y);
        }

        public void LoadGameBoardObjects(char[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    GameObject gameObject = null;

                    switch (grid[x, y])
                    {
                        case '@':
                            gameObject = new Powerup();
                            break;
                        case '.':
                            gameObject = new Bubble();
                            break;
                    }

                    if (gameObject == null)
                        continue;

                    gameObject.Position = new Vector2(x, y);
                    this.Add(gameObject);

                }
            }
        }
    }
}
