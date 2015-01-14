using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        public Pacman Pacman { get; private set; }
        public GhostHouse GhostHouse { get; private set; }

        public Level()
        {
        
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.Pacman.Direction = inputHelper.GetDirectionalInput();
        }

        public void LoadLevel(string path)
        {
            FileReader level = new FileReader("Content/level1.txt");

            char[,] levelGrid = level.ReadGrid("level");
            GameBoard gameBoard = GameBoard.CopyDimensions(levelGrid);
            this.Add(gameBoard);

            for (int x = 0; x < gameBoard.Size.X; x++)
            {
                for (int y = 0; y < gameBoard.Size.Y; y++)
                {
                    GameTile tile = new Ground();

                    switch (levelGrid[x, y])
                    {
                        case '#':
                            tile = new Wall();
                            break;
                        case 'o':
                            tile = new InvisibleWall();
                            break;
                        case '.':
                            Bubble b = new Bubble();
                            b.Position = new Vector2(x, y);
                            this.Add(b);
                            break;
                        default:
                            tile = new Ground();
                            break;
                    }

                    gameBoard.Add(tile, x, y);
                }
            }

            this.Pacman = new Pacman();
            Pacman.Position = level.ReadVector("pacman");
            this.Add(Pacman);

            GhostHouse = new GhostHouse();
            this.Add(GhostHouse);

            Blinky blinky = new Blinky(Pacman);
            blinky.Position = level.ReadVector("blinky");
            blinky.Direction = Vector2.UnitX;
            GhostHouse.Add(blinky);
            
        }
    }
}
