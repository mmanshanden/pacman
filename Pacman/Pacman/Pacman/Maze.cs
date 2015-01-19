using _3dgl;
using Base;
using System.Collections.Generic;

namespace Pacman
{
    class Maze : GameBoard
    {
        List<GameTile> tiles;

        public Maze(int width, int height) 
            : base(width, height)
        {
            this.tiles = new List<GameTile>();
        }

        public void Add(GameTile gameTile, int x, int y)
        {
            this.tiles.Add(gameTile);
            base.Add(gameTile, x, y);
        }

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            foreach (GameTile tile in this.tiles)
                tile.Load(mb);

            Game.DrawManager.ModelLibrary.EndModel("maze");
        }

        public static Maze CopyDimensions(char[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            return new Maze(width, height);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            Game.DrawManager.DrawModel("maze");
        }
    }
}
