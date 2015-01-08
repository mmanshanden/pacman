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

            base.gameBoard = new GameBoard(width, height);
                        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameTile tile = null;

                    switch (charGrid[x, y])
                    {
                        case '#':
                            tile = new Wall();
                            break;
                        case 'o':
                            tile = new Ground();
                            break;
                        default:
                            tile = new Ground();
                            break;
                    }

                    this.gameBoard.Add(tile, x, y);
                }
            }

            this.pacman = new Pacman();
            pacman.Position = level.ReadVector("pacman");
            this.Add(pacman);

           
        }
    }
}
