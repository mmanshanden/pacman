﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Base
{
    /// <summary>
    /// A movable game object
    /// </summary>
    public class GameCharacter : GameObject
    {
        #region Movement
        private Vector2 direction;

        public virtual Vector2 Direction
        {
            get { return this.direction; }
            set
            {
                // movement can only be aligned to x or y axis
                this.direction = Collision.ToDirectionVector(value);
            }
        }
        public float Speed
        {
            get;
            set;
        }
        public Vector2 Velocity
        {
            get { return this.Direction * this.Speed; }
        }
        #endregion

        protected GameWorld World
        {
            get { return this.Parent as GameWorld; }
        }
        protected GameBoard Board
        {
            get
            {
                GameWorld world = this.World;
                if (this.World != null && this.World.GameBoard != null)
                    return this.World.GameBoard;

                return null;
            }
        }

        public GameCharacter()
        {
            this.Direction = Vector2.Zero;
            this.Speed = 6;
        }

        /// <summary>
        /// Gets called when an gamecharacter will move into an invalid direction.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="tile"></param>
        public virtual void Collision_InvalidDirection(GameBoard board, GameTile tile)
        {
            this.Direction = Vector2.Zero;
        }

        /// <summary>
        /// Gets called when a gamecharacter crosses a junction of non-collidable tiles.
        /// (3 or more neighbors).
        /// </summary>
        /// <param name="board"></param>
        /// <param name="tile"></param>
        public virtual void Collision_Junction(GameBoard board, GameTile tile)
        {
            
        }

        /// <summary>
        /// Handles movement and collision checks.
        /// </summary>
        protected virtual void Move(GameBoard board, GameTile tile, float dt)
        {
            Vector2 junction = tile.Center;

            float v, j, p;
            GameTile next;

            // set velocity, junction, position
            // and next tile based on direction.
            if (this.Direction.X != 0)
            {
                v = this.Velocity.X;
                p = this.Center.X;
                j = junction.X;

                if (Direction.X > 0)
                    next = tile.Right;
                else
                    next = tile.Left;

                junction.Y = this.Center.Y;
            }
            else
            {
                v = this.Velocity.Y;
                p = this.Center.Y;
                j = junction.Y;

                if (Direction.Y > 0)
                    next = tile.Bottom;
                else
                    next = tile.Top;

                junction.X = this.Center.X;
            }

            if (next == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            float t = Collision.SolveForX(v, p, j);

            // will we cross junction?
            if (t < 0 || t > dt)
            {
                // nope..!
                this.Position += this.Velocity * dt;
                return;
            }

            // we crossed junction
            this.Center = junction;

            if (next.IsCollidable(this))
            {
                this.Collision_InvalidDirection(board, tile);
                this.Position += this.Velocity * (dt - t);
                return;
            }

            switch (tile.GetNeighbourCount(this))
            {
                case 3:
                case 4:
                    this.Collision_Junction(board, tile);
                    break;
            }

            this.Position += this.Velocity * (dt - t);
        }

        public override void Update(float dt)
        {
            // pre movement checks.
            if (this.Velocity == Vector2.Zero)
                return;

            if (this.World == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            GameBoard board = this.Board;

            if (board == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }

            GameTile tile = board.GetTile(this);

            if (tile == null)
            {
                this.Position += this.Velocity * dt;
                return;
            }
            
            // do collision check move
            this.Move(board, tile, dt);
        }

    }
}
