using Base;
using Microsoft.Xna.Framework;
using System;

namespace Pacman
{
    class Ghost : GameCharacter
    {
        public enum States
        {
            Chase,
            Scatter,
            Dead,
            Frightened
        }

        public GhostHouse GhostHouse
        {
            get
            {
                return this.Parent as GhostHouse;
            }
        }



        public static Random Random
        {
            get;
            private set;
        }

        public Vector2 Target
        {
            get;
            set;
        }

        public States State
        {
            get;
            set;
        }

        private float frightenedTimer, scatterTimer;
        private int scatterCount;
        States storedState;

        public Ghost()
        {
            this.State = States.Chase;
            Ghost.Random = new Random();
            this.scatterTimer = 0; 
        }

        #region Collision Events
        public override void Collision_InvalidDirection(GameBoard board, GameTile tile)
        {
            if (tile.GetNeighbourCount(this) == 1)
                this.Direction = Vector2.Zero;

            this.Collision_Junction(board, tile);
        }
        public override void Collision_Junction(GameBoard board, GameTile tile)
        {
            Vector2 move = this.Direction;
            float min = float.PositiveInfinity;

            foreach (GameTile neighbour in tile.GetNeighbourList(this))
            {
                Vector2 direction = neighbour.Position - tile.Position;

                // ghosts never go back, so we skip that direction
                if (direction * -1 == this.Direction)
                    continue;

                Vector2 center = neighbour.Center;
                float distance = Vector2.Distance(center, this.Target);

                if (distance < min)
                {
                    min = distance;
                    move = direction;
                }

            }

            this.Direction = move;
        }
        public virtual void Collision_Target(GameBoard board, GameTile tile)
        {
            if (this.Position == GhostHouse.Entry && this.State == States.Dead)
            {
                this.State = States.Chase;
            }

        }
        #endregion

        protected override void Move(GameBoard board, GameTile tile, float dt)
        {
            float v, j, p;

            // set velocity, junction, position
            // and next tile based on direction.
            if (this.Target.Y == this.Center.Y)
            {
                v = this.Velocity.X;
                p = this.Center.X;
                j = this.Target.X;
            }
            else if (this.Target.X == this.Center.X)
            {
                v = this.Velocity.Y;
                p = this.Center.Y;
                j = this.Target.Y;
            }
            else
            {
                base.Move(board, tile, dt);
                return;
            }

            float t = Collision.SolveForX(v, p, j);

            // will we reach target?
            if (t < 0 || t > dt)
            {
                // nope..!

                base.Move(board, tile, dt);
                return;
            }

            // we crossed junction
            this.Center = this.Target;
            this.Collision_Target(board, tile);
            Console.WriteLine("Target event");
            base.Move(board, tile, dt - t);
        }


        public void Frighten()
        {
            storedState = this.State; 
            this.State = States.Frightened;
            this.frightenedTimer = 6;
        }


        public States ScatterSwitch(float dt)
        {
            scatterTimer -= dt;
            Console.WriteLine(scatterTimer.ToString()); 

            if (scatterTimer <= 0) 
            {
                switch (scatterCount)
                {
                    case 0:
                    case 2:
                        scatterTimer = 7;
                        scatterCount++;
                        return States.Scatter;
                    case 4:
                    case 6:
                        scatterTimer = 5;
                        scatterCount++;
                        return States.Scatter;
                    case 1:
                    case 3:
                    case 5:
                        scatterTimer = 20;
                        scatterCount++;
                        return States.Chase;

                    default:
                        return States.Chase;
                }
            }

            return this.State; 
        }

        public virtual Vector2 GetTarget(States state)
        {
            Level level = (Level)this.Parent.Parent; 

            switch (state)
            {
                case States.Dead:
                    // set t
                    return level.GhostHouse.Entry; 
                case States.Frightened:
                    return new Vector2(Random.Next((int)level.GameBoard.Size.X), Random.Next((int)level.GameBoard.Size.Y));
                case States.Scatter:
                    return new Vector2(2, 2); // have to chance this later
            }

            return Vector2.Zero; 

        }

        public override void Update(float dt)
        {
            

            switch (this.State)
            {
                case States.Chase:
                case States.Scatter:
                    this.Speed = 6;
                    break;
                case States.Frightened:
                    this.Speed = 4;
                    break;
                case States.Dead:
                    this.Speed = 20;
                    break;
            }

            if (this.State == States.Frightened)
                frightenedTimer -= dt;

            if (this.frightenedTimer < 0 && this.State == States.Frightened)
                this.State = storedState;  

            this.Target = GetTarget(this.State);

            if (this.State != States.Frightened && this.State != States.Dead)
                this.State = ScatterSwitch(dt); 
            




            base.Update(dt);
        }
    }
}
