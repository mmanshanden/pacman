using System;
using Microsoft.Xna.Framework;

namespace Base
{
    public class Collision
    {
        #region Vector presets
        public static Vector2 HalfVector
        {
            get
            {
                return new Vector2(0.5f, 0.5f);
            }
        }
        public static Vector2 Left
        {
            get
            {
                return Vector2.UnitX * -1;
            }
        }
        public static Vector2 Right
        {
            get
            {
                return Vector2.UnitX;
            }
        }
        public static Vector2 Up
        {
            get
            {
                return Vector2.UnitY * -1;
            }
        }
        public static Vector2 Down
        {
            get
            {
                return Vector2.UnitY;
            }
        }
        #endregion

        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }
        public static Vector2 ToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
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
        // axis seperated!
        public static Vector2 IntersectionTime(Vector2 p, Vector2 v, Vector2 j)
        {
            Vector2 result = new Vector2();
            result.X = SolveForX(v.X, p.X, j.X);
            result.Y = SolveForX(v.Y, p.Y, j.Y);
            return result;
        }
        public static float GetSmallestPositive(float a, float b)
        {
            float smallest = float.PositiveInfinity;

            if (a >= 0 && a < smallest)
                smallest = a;

            if (b >= 0 && b < smallest)
                smallest = b;

            return smallest;
        }
    }
}
