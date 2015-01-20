using Base;
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
            Maze maze = Maze.CopyDimensions(grid);

            for (int x = 0; x < maze.Size.X; x++)
            {
                for (int y = 0; y < maze.Size.Y; y++)
                {
                    // standard ground tile
                    GameTile tile = new Ground();

                    switch (grid[x, y])
                    {
                        case '#':
                            tile = new Wall();
                            break;
                        case '-':
                            tile = new GhostHouseEntry();
                            break;
                        case 'o':
                            tile = new GhostHouseVoid();
                            break;
                        case 'X':
                            tile = new Boundary();
                            break;
                        case 'G':
                            tile = new GhostHouseWall();
                            break;
                    }

                    maze.Add(tile, x, y);
                }
            }

            this.Add(maze);

            Game.DrawManager.Camera.SetTarget((this.GameBoard.Size / 2) + Vector2.UnitY * 5);
            Game.DrawManager.Camera.SetCameraHeight(0.5f);
            Game.DrawManager.Camera.Rho = 1.25f;
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
