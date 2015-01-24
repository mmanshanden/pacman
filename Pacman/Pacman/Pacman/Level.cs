using Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        private List<GhostHouse> ghosthouses;
        private List<Bubble> bubbles;
        private List<Powerup> powerups;

        private float countdown;

        public Player Player { get; private set; }
        public List<GhostHouse> GhostHouses
        {
            get { return this.ghosthouses; }
        }
        
        public Level()
        {
            this.ghosthouses = new List<GhostHouse>();
            this.bubbles = new List<Bubble>();
            this.powerups = new List<Powerup>();
            this.countdown = 3.1f;
        }

        public void ResetCountdown()
        {
            this.countdown = 3;
        }

        public List<Vector2> GetBubbles()
        {
            List<Vector2> result = new List<Vector2>();

            foreach (Bubble b in this.bubbles)
                result.Add(b.Position);

            return result;
        }
        public List<Vector2> GetPowerUps()
        {
            List<Vector2> result = new List<Vector2>();

            foreach (Powerup p in this.powerups)
                result.Add(p.Position);

            return result;
        }

        public void Add(Player player)
        {
            this.Player = player;
            base.Add(player);            
        }

        public void Add(GhostHouse ghostHouse)
        {
            this.ghosthouses.Add(ghostHouse);
            base.Add(ghostHouse);
        }

        public void Add(Bubble bubble)
        {
            this.bubbles.Add(bubble);
            base.Add(bubble);
        }

        public void Add(Powerup powerup)
        {
            this.powerups.Add(powerup);
            base.Add(powerup);
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
                        case '*':
                            tile = new GhostHouseEntry();
                            break;
                        case 'o':
                            tile = new GhostHouseVoid();
                            break;
                        case 'G':
                            tile = new GhostHouseWall();
                            break;
                        case ' ':
                            tile = null;
                            break;
                        case '-':
                            tile = new Boundary(Boundary.Orientations.Horizontal);
                            break;
                        case '|':
                            tile = new Boundary(Boundary.Orientations.Vertical);
                            break;
                        case '+':
                            tile = new Boundary(Boundary.Orientations.Corner);
                            break;
                    }

                    maze.Add(tile, x, y);
                }
            }

            this.Add(maze);
        }

        public void LoadGameBoardObjects(char[,] grid)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    switch (grid[x, y])
                    {
                        case '.':
                            Bubble bubble = new Bubble();
                            bubble.Position = new Vector2(x, y);
                            this.Add(bubble);
                            break;
                        case '@':
                            Powerup powerup = new Powerup();
                            powerup.Position = new Vector2(x, y);
                            this.Add(powerup);
                            break;
                    }
                }
            }
        }

        public override void Update(float dt)
        {
            if (this.countdown == 3.1f)
                    Game.SoundManager.PlaySoundEffect("level_start");

            Game.Camera.Target = (this.GameBoard.Size / 2);
            Game.Camera.Target += Vector2.UnitY * 3;
            Game.Camera.SetCameraHeight(2);
            
            countdown -= dt;

            if (this.countdown < 0)
                base.Update(dt);
            else
            {
                Game.Camera.Rho = MathHelper.PiOver2 - 0.08f;
                Game.Camera.Zoom = 26;
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);

            int countdown = (int)this.countdown + 1;

            if (this.countdown > 0)
                drawHelper.DrawStringBig(countdown.ToString(), Vector2.One * 0.5f, DrawHelper.Origin.Center, Color.White);
        }
    }
}
