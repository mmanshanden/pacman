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
            this.GameBoard = new GameBoard();
        }

        public override void Draw(DrawHelper drawHelper)
        {
            for (int x = 0; x < this.GameBoard.Size.X; x++)
            {
                for (int y = 0; y < this.GameBoard.Size.Y; y++)
                {
                    drawHelper.Translate(x, y);

                    switch (this.GameBoard.GetTileValue(new Point(x, y)))
                    {
                        case 1:
                            drawHelper.DrawBox(Color.Blue);
                            break;
                    }


                    drawHelper.Translate(-x, -y);
                }
            }
            
            base.Draw(drawHelper);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.pacman.Direction = inputHelper.GetDirectionalInput();
        }

        public void LoadLevel(string path)
        {
            FileReader level = new FileReader("Content/level1.txt");

            char[,] charGrid = level.ReadGrid("level");
            short[,] grid = new short[charGrid.GetLength(0), charGrid.GetLength(1)];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    switch (charGrid[x, y])
                    {
                        case '#':
                            grid[x, y] = 1;
                            break;
                        default:
                            grid[x, y] = 0;
                            break;
                    }
                }
            }

            this.pacman = new Pacman();
            this.pacman.Position = level.ReadVector("pacman");
            this.pacman.Speed = 6;
            this.Add(pacman);

            this.GameBoard.SetGrid(grid, new short[] { 1, 2 });
        }
    }
}
