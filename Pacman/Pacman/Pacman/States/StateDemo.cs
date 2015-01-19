using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateDemo : IGameState
    {
        FileReader levelFile;
        Level level;

        public StateDemo()
        {
            this.levelFile = new FileReader("Content/Levels/level1.txt");
            this.level = new Level();

            this.level.LoadGameBoard(levelFile.ReadGrid("level"));
            this.level.LoadGameBoardObjects(levelFile.ReadGrid("level"));

            Player player = new Player();
            player.Spawn = this.levelFile.ReadVector("player_position");
            this.level.Add(player);

            GhostHouse ghostHouse = new GhostHouse();
            ghostHouse.Entry = this.levelFile.ReadVector("ghosthouse_entry");
            ghostHouse.Center = this.levelFile.ReadVector("ghosthouse_center");
            ghostHouse.AddPacman(player);
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
            this.level.HandleInput(inputHelper);

            Vector2 rs = inputHelper.RightStickVector();

            if (rs == Vector2.Zero)
                return;

            Game.DrawManager.Camera.Phi += rs.X * 0.1f;
            Game.DrawManager.Camera.Rho += rs.Y * -0.1f;
        }

        public IGameState TransitionTo()
        {
            return this;
        }

        public void Update(float dt)
        {
            this.level.Update(dt);
        }

        public void Draw(DrawHelper drawHelper)
        {
            drawHelper.Scale(13, 13);
            this.level.Draw(drawHelper);
            drawHelper.Scale(1 / 13f, 1 / 13f);
        }

    }
}
