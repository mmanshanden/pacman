using Base;

namespace Pacman
{
    class Maze : GameBoard
    {
        public Maze(int width, int height) 
            : base(width, height)
        {

        }

        public static Maze CopyDimensions(char[,] grid)
        {
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            return new Maze(width, height);
        }
    }
}
