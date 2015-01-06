using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        Pacman pacman;

        public Level()
        {
        
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.pacman.Direction = inputHelper.GetDirectionalInput();
        }

        public void LoadLevel(string path)
        {
            FileReader level = new FileReader("Content/level1.txt");

            char[,] charGrid = level.ReadGrid("level");

            int width = charGrid.GetLength(0);
            int height = charGrid.GetLength(1);

            this.GameBoard = new GameBoard(width, height);

            short[,] grid = new short[width, height];
            
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject tile = null;

                    switch (charGrid[x, y])
                    {
                        case '#':
                            grid[x, y] = 1;
                            tile = new Wall();
                            break;
                        case 'o':
                            grid[x, y] = 2;
                            tile = new Ground();
                            break;
                        default:
                            grid[x, y] = 0;
                            tile = new Ground();
                            break;
                    }

                    this.GameBoard.Add(tile, x, y);
                }
            }

            this.pacman = new Pacman();
            this.pacman.Position = level.ReadVector("pacman");
            this.pacman.Speed = 6;
            this.Add(pacman);

            if (level.ReadString("ghosts").Contains("blinky"))
            {
                Blinky b = new Blinky(level.ReadVector("blinky"), pacman);
                b.Speed = 5.5f;
                b.Direction = Vector2.UnitX;
                this.Add(b);
            }

            this.GameBoard.SetGrid(grid, new short[] { 1, 2 });
        }
    }
}
