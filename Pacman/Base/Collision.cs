using System;
using Microsoft.Xna.Framework;

namespace Base
{
    public class Collision
    {
        // Sin(pi / 4)
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

        /// <summary>
        /// Converts a Vector to a Point.
        /// </summary>
        /// <param name="vector">Vector to convert</param>
        /// <returns></returns>
        public static Point ToPoint(Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        /// <summary>
        /// Converts a Point to a Vector
        /// </summary>
        /// <param name="point">Point to convert</param>
        /// <returns></returns>
        public static Vector2 ToVector(Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        /// <summary>
        /// Solves the x value in the equation y = a*x + b
        /// </summary>
        /// <returns></returns>
        public static float SolveForX(float a, float b, float y)
        {
            return (y - b) / a;
        }

        /// <summary>
        /// Converts a vector pointing in any direction to
        /// a vector pointing in either the up, down, left
        /// or right direction.
        /// </summary>
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
