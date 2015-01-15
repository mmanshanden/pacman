using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        private FileReader levelFile;
        public Player Player { get; private set; }
        public GhostHouse GhostHouse { get; private set; }

        public Level(string filePath)
        {
            this.levelFile = new FileReader(filePath);
        }


        public void Add(GameObject gameObject, string levelFileKey)
        {
            gameObject.Position = this.levelFile.ReadVector(levelFileKey);
            base.Add(gameObject);
        }

        public void Add(Player player, string levelFileKey = "")
        {
            this.Player = player;

            if (levelFileKey == "")
                base.Add(player);
            else
                this.Add(player as GameObject, levelFileKey);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.Player.Direction = inputHelper.GetDirectionalInput();
        }

        public void LoadGameBoard(string levelFileKey)
        {
            // get level grid from file
            char[,] levelGrid = levelFile.ReadGrid("level");

            // initialize gameboard and add board to level
            GameBoard gameBoard = GameBoard.CopyDimensions(levelGrid);
            this.Add(gameBoard);


            for (int x = 0; x < gameBoard.Size.X; x++)
            {
                for (int y = 0; y < gameBoard.Size.Y; y++)
                {
                    // standard ground tile
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
                        case '@':
                            Powerup p = new Powerup();
                            p.Position = new Vector2(x, y);
                            this.Add(p);
                            break;
                        default:
                            tile = new Ground();
                            break;
                    }

                    gameBoard.Add(tile, x, y);
                }
            }           
        }
    }
}
