using Base;
using Microsoft.Xna.Framework;
using Network;
using System.Collections.Generic;
using System.IO;

namespace Pacman
{
    class Level : GameWorld
    {
        private List<GhostHouse> ghosthouses;
        private float countdown;

        public Player Player { get; private set; }
        public List<GhostHouse> GhostHouses
        {
            get { return this.ghosthouses; }
        }
        
        public Level()
        {
            this.ghosthouses = new List<GhostHouse>();
            this.countdown = 3.1f;
        }

        public void ResetCountdown()
        {
            this.countdown = 3;
        }

        // Returns list of all Powerups in the level
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

        // Returns list of all Powerups in the Level
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
            this.ghosthouses.Add(ghostHouse);
            base.Add(ghostHouse);
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

                    // Add tiles according to symbols in .txt file
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

        // Add game objects to the level according to symbols in .txt file
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
            // Play start sound
            if (this.countdown == 3.1f)
                    Game.SoundManager.PlaySoundEffect("level_start");

            Game.Camera.Target = (this.GameBoard.Size / 2);
            Game.Camera.Target += Vector2.UnitY * 3;
            Game.Camera.SetCameraHeight(2);
            
            // If countdown is over update the Level
            if (this.countdown < 0)
                base.Update(dt);
            else
                countdown -= dt;
        }

        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);

            int countdown = (int)this.countdown + 1;
            // Draw countdown on the screen
            if (this.countdown > 0)
                drawHelper.DrawStringBig(countdown.ToString(), Vector2.One * 0.5f, DrawHelper.Origin.Center, Color.White);
        }

        // Returns a mapMessage containing all bubbles and powerups 
        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            MapMessage mapMessage = (MapMessage)cmsg;

            mapMessage.Bubbles = this.GetBubbles();
            mapMessage.PowerUps = this.GetPowerUps();

            return mapMessage as NetMessageContent;
        }

        public override void UpdateObject(NetMessageContent cmsg)
        {
            // basically, we overwrite the bubbles and powerups
            // stored in the level with the data we get from
            // the server

            MapMessage mmsg = (MapMessage)cmsg;

            // clear list from all bubbles and powerups
            for (int i = base.gameObjects.Count - 1; i >= 0; i--)
            {
                if (base.gameObjects[i] is Bubble || base.gameObjects[i] is Powerup)
                    base.gameObjects.RemoveAt(i);
            }

            // refill lists
            foreach (Vector2 b in mmsg.Bubbles)
            {
                Bubble bubble = new Bubble();
                bubble.Position = b;
                base.Add(bubble);
            }

            foreach (Vector2 p in mmsg.PowerUps)
            {
                Powerup powerup = new Powerup();
                powerup.Position = p;
                this.Add(powerup);
            }
        }
    }
}
