using Base;
using Microsoft.Xna.Framework;
namespace Pacman
{
    class Ghost : GameObject
    {
        public enum States
        {
            Chase,
            Scatter,
            Dead,
        }

        public Vector2 Target
        {
            get;
            set;
        }

        Vector2 spawn;

        public Ghost(Vector2 spawn)
        {
            this.spawn = spawn;
            this.Position = spawn;
        }

        public override void Collision_InvalidDirection(GameBoard gameBoard)
        {
            this.Collision_Junction(gameBoard);
        }

        public override void Collision_Junction(GameBoard gameBoard)
        {
            Vector2 move = this.Direction;
            float min = float.PositiveInfinity;

            foreach (Point neighbour in gameBoard.GetNeighbourTiles(this))
            {
                Vector2 direction = new Vector2();
                direction.X = neighbour.X - Tile.X;
                direction.Y = neighbour.Y - Tile.Y;

                // ghosts never go back, so we skip that direction
                if (direction * -1 == this.Direction)
                    continue;

                Vector2 center = new Vector2(neighbour.X, neighbour.Y) + Vector2.One * 0.5f;
                float distance = Vector2.Distance(center, this.Target);

                if (distance < min)
                {
                    min = distance;
                    move = direction;
                }

            }

            this.Direction = move;
        }
    }
}
