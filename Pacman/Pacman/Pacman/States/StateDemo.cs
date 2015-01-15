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
            player.Position = this.levelFile.ReadVector("player_position");
            this.level.Add(player);

            GhostHouse ghostHouse = new GhostHouse();
            ghostHouse.AddPacman(player);
            this.level.Add(ghostHouse);

            Blinky blinky = new Blinky();
            blinky.Position = this.levelFile.ReadVector("blinky_position");
            blinky.Direction = new Vector2(0, 1);
            ghostHouse.Add(blinky);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            this.level.HandleInput(inputHelper);
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
