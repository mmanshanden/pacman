using System;
using Microsoft.Xna.Framework;

namespace Base
{
    class Collision
    {
        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
        public static Vector2 ToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }
        public static Vector2 TileCenter(Point tile)
        {
            return ToVector(tile) + Vector2.One * 0.5f;
        }
        public static float SolveForX(float a, float b, float y)
        {
            return (y - b) / a;
        }
        public static Vector2 ModuloVector(Vector2 vector, float modulo)
        {
            vector.X %= modulo;
            vector.Y %= modulo;
            return vector;
        }
    }
}
