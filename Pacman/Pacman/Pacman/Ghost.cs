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

        public Level Level
        {
            get;
            set;
        }
        public GhostHouse GhostHouse
        {
            get;
            set;
        }

        public Vector2 Scatter
        {
            get;
            set;
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

        public float totalTime;
        public float frightenedTime;

        public Ghost()
        {
            this.State = States.Chase;
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
            if (this.Center == GhostHouse.Entry && this.State == States.Dead)
            {
                this.State = States.Chase;
            }
        }
        #endregion

        public override void Collision_GameObject(GameObject gameObject)
        {
            if (this.State == States.Frightened && gameObject is Pacman)
                this.State = States.Dead;
        }

        #region Move - Don't touch
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
        #endregion

        public void Frighten()
        {
            this.State = States.Frightened;
            this.frightenedTime = 6;
        }
    
        public virtual Vector2 GetTarget(States state)
        {
            switch (state)
            {
                case States.Dead:
                    return this.GhostHouse.Entry; 

                case States.Frightened:
                    Vector2 size = this.World.GameBoard.Size;
                    Vector2 random = new Vector2();

                    random.X = Game.Random.Next((int)size.X);
                    random.Y = Game.Random.Next((int)size.Y);
                    return random;

                case States.Scatter:
                    return this.Scatter;
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
            {
                this.frightenedTime -= dt;

                if (frightenedTime < 0)
                    this.State = States.Scatter;
            }

            // switch between scatter and chase
            if (this.State == States.Chase || this.State == States.Scatter)
            {
                // 84 is the number of seconds after which the chase
                // state will be indefinite
                if (this.totalTime <= 84)
                    this.totalTime += dt;

                // start with scatter
                this.State = States.Scatter;

                if (this.totalTime > 7)
                    this.State = States.Chase;

                if (this.totalTime > 27)
                    this.State = States.Scatter;

                if (this.totalTime > 34)
                    this.State = States.Chase;

                if (this.totalTime > 54)
                    this.State = States.Scatter;

                if (this.totalTime > 59)
                    this.State = States.Chase;

                if (this.totalTime > 79)
                    this.State = States.Scatter;

                if (this.totalTime > 84)
                    this.State = States.Chase;
            }

            this.Target = this.GetTarget(this.State);

            base.Update(dt);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);

            switch(this.State)
            {
                case States.Dead:
                    drawHelper.DrawBox(Color.DarkBlue);
                    break;

                case States.Frightened:
                    Color color = Color.DarkBlue;

                    if (this.frightenedTime < 2 &&
                        this.frightenedTime % 0.4f < 0.2f)
                    {
                        color = Color.White;
                    }

                    drawHelper.DrawBox(color);
                    break;
            }

            drawHelper.Translate(-this.Position);
        }
    }
}
