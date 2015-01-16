using System;
using Microsoft.Xna.Framework;

namespace Base
{
    public class Collision
    {
        private const float QuarterCircle = 0.70710678118f;

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

        public static Vector2 ToDirectionVector(Vector2 vector)
        {
            if (vector.Length() < 0.2f)
                return Vector2.Zero;
                     
            vector.Normalize();

            if (vector.Y > 0)
            {
                if (vector.X > QuarterCircle)
                    return Right;

                if (vector.X < -QuarterCircle)
                    return Left;

                return Down;

            }
            else
            {
                if (vector.X > QuarterCircle)
                    return Right;

                if (vector.X < -QuarterCircle)
                    return Left;

                return Up;
            }
            
        }

    }
}
