using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Network;
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
            Frightened,
            Wait,
            Leave
        }
        
        private Vector2 spawn;

        #region Properties
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
        public Vector2 Spawn
        {
            get { return this.spawn; }
            set
            {
                this.Position = value;
                this.spawn = value;
            }
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
        #endregion

        protected float totalTime;
        protected float frightenedTime;
        protected float waitTime;
        protected float waitTimer; 

        public Ghost()
        {
            this.State = States.Wait;
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
            // If a ghost is leaving and is on the ghosthouse entry set its state to Chase.
            if (this.Center == GhostHouse.Entry && this.State == States.Leave)
            {
                this.State = States.Chase;
            }

            // If ghost is dead and is on the Ghosthouse entry set its State to wait.
            if (this.Center == GhostHouse.Entry && this.State == States.Dead)
            {
                this.State = States.Wait;

                //Let ghost wait 3 seconds before leaving ghosthouse again
                this.waitTimer = 3; 
            }

        }
        #endregion

        public override void Collision_GameObject(GameObject gameObject)
        {
            if (this.State == States.Frightened && gameObject is Pacman)
            {
                this.State = States.Dead;
                this.totalTime = 0;

                if (gameObject is Player)
                    Game.SoundManager.PlaySoundEffect("ghost_dead");
            }
        }

        #region Move - Don't touch
        protected override void Move(GameBoard board, GameTile tile, float dt)
        {
            // non ai ghost
            if (this.GhostHouse == null)
            {
                base.Move(board, tile, dt);
                return;
            }

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
            base.Move(board, tile, dt - t);
        }
        #endregion
        
        // If the ghost is outside the ghosthouse while pacman eats a powerup
        // set it's state to frightened
        public void Frighten()
        {
            if (this.State == States.Scatter || this.State == States.Chase || this.State == States.Frightened)
            {
                this.State = States.Frightened;
                this.frightenedTime = this.GhostHouse.FrightenedDuration;
            }
        }
    
        // Sets ghost's target location depending on its state
        public virtual Vector2 GetTarget(States state)
        {
            switch (state)
            {
                case States.Leave:
                case States.Dead:
                    return this.GhostHouse.Entry;

                case States.Wait:
                    return this.Spawn;

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

        // Returns ghostmessage containing information about the ghost
        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            GhostMessage gmsg = new GhostMessage();
            NetMessageContent.CopyOver(cmsg, gmsg);

            gmsg.Position = this.Position;
            gmsg.Direction = this.Direction;
            gmsg.Speed = this.Speed;
            gmsg.Target = this.Target;
            gmsg.State = (byte)this.State;
            gmsg.FrightenTime = this.frightenedTime;

            return gmsg;
        }

        // Update Ghost according to Data received
        // from the server
        public override void UpdateObject(NetMessageContent cmsg)
        {
            GhostMessage gmsg = (GhostMessage)cmsg;

            this.Position = gmsg.Position;
            this.Direction = gmsg.Direction;
            this.Speed = gmsg.Speed;
            this.Target = gmsg.Target;
            this.State = (States)gmsg.State;
            this.frightenedTime = gmsg.FrightenTime;
        }

        // Reset Ghost to begin state
        public void Respawn()
        {
            this.Position = this.Spawn;
            this.Direction = Vector2.UnitY * -1;
            this.State = States.Wait;
            this.waitTimer = this.waitTime; 
            this.totalTime = 0;
        }
        
        public override void Update(float dt)
        {   
            // non ai ghost
            if (this.GhostHouse == null)
            {
                base.Update(dt);
                return;
            }

            // Set speed of ghost depending on its state
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
                case States.Leave:
                case States.Wait:
                    this.Speed = 5;
                    break;
            }

            if (this.State == States.Frightened)
            {
                this.frightenedTime -= dt;

                if (frightenedTime < 0)
                    this.State = States.Scatter;
            }

            if (this.State == States.Wait)
            {
                this.waitTimer -= dt;
                if (waitTimer < 0)
                {
                    this.State = States.Leave; 
                }
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

        public static void Load(ModelLibrary modelLibrary)
        {
            // load scared
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/scared", 15);

            modelLibrary.EndModel("scared");


            // load scared blink
            mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.PrimitiveBatch.Translate(Vector3.One * -0.25f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.5f);

            mb.BuildFromTexture("voxels/scaredblink", 15);

            modelLibrary.EndModel("scared_blink");
        }

        // Draw 3D model of ghosts depending on their State
        public override void Draw(DrawManager drawManager)
        {
            float radians = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            drawManager.RotateOver(radians, Vector2.One * 0.5f);
            drawManager.Translate(this.Position.X, this.Position.Y);

            switch (this.State)
            {
                case States.Dead:
                    drawManager.DrawModel("scared_blink");
                    break;

                case States.Frightened:
                    if (this.frightenedTime < 2 && this.frightenedTime % 0.4f < 0.2f)
                        drawManager.DrawModel("scared_blink");
                    
                    else
                        drawManager.DrawModel("scared");

                    break;
            }


            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-radians, Vector2.One * 0.5f);
        }
    }
}
