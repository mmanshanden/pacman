using Base;
using Network;
using Microsoft.Xna.Framework;
using _3dgl;

namespace Pacman
{
    class Pacman : GameCharacter
    {
        public const float AnimationTime = 0.32f;

        private float time;
        private bool closed;
        private float rotation;
        
        public int Lives
        {
            get;
            set; 
        }

        public int Score
        {
            get;
            set; 
        }

        private Vector2 spawn;

        public Vector2 Spawn
        {
            get { return this.spawn; }
            set
            {
                this.Position = value;
                this.spawn = value;
            }
        }

        public Pacman()
        {
            this.Lives = 3;
            this.Score = 0;

            this.time = AnimationTime;
            this.closed = false;
        }

        public override void Collision_GameObject(GameObject gameObject)
        {
            if (gameObject is Bubble)
                this.Score++;
                
            Ghost ghost = gameObject as Ghost;
            if (ghost != null)
            {
                if (ghost.State == Ghost.States.Chase || ghost.State == Ghost.States.Scatter)
                {
                    this.Dead(); 
                }
            }

            if (gameObject is Bubble)
            {
                this.Speed = 0;
            }
        }

        public virtual void Dead()
        {
            this.Lives--;
            this.Position = this.spawn;
            Level level = (Level)this.Parent;

            if (level.GhostHouse != null)
                level.GhostHouse.ResetGhosts();
            level.countdown = 3;
        }

        public override void Update(float dt)
        {
            if (this.Lives < 1)
                this.Position = Vector2.One * -20;

            if (this.Velocity != Vector2.Zero)
                this.time -= dt;

            this.closed = (this.time < AnimationTime / 2);

            if (this.time < 0)
                this.time = AnimationTime;

            base.Update(dt);

            this.Speed = 6;
        }

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.8f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            mb.BuildFromTexture("voxels/pacmanopen", 16);
            Game.DrawManager.ModelLibrary.EndModel("pacman_open");

            
            
            mb = Game.DrawManager.ModelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.8f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            mb.BuildFromTexture("voxels/pacmanclosed", 16);
            Game.DrawManager.ModelLibrary.EndModel("pacman_closed");
        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.Lives < 1)
                return;
            
            if (this.Velocity != Vector2.Zero)
                this.rotation = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            Game.DrawManager.RotateOver(this.rotation, Vector2.One * 0.5f);
            Game.DrawManager.Translate(this.Position.X, this.Position.Y);

            if (this.closed)
                Game.DrawManager.DrawModel("pacman_closed");
            else
                Game.DrawManager.DrawModel("pacman_open");

            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
            Game.DrawManager.RotateOver(-rotation, Vector2.One * 0.5f);
        }


        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            PlayerMessage pmsg = new PlayerMessage();
            NetMessageContent.CopyOver(cmsg, pmsg);
            
            pmsg.Position = this.Position;
            pmsg.Direction = this.Direction;
            pmsg.Speed = this.Speed;
            pmsg.Lives = this.Lives;
            pmsg.Score = this.Score;

            return pmsg;
        }

        public override void UpdateObject(NetMessageContent cmsg)
        {
            PlayerMessage pmsg = (PlayerMessage)cmsg;

            this.Position = pmsg.Position;
            this.Direction = pmsg.Direction;
            this.Speed = pmsg.Speed;

            if (pmsg.Lives < this.Lives)
                this.Lives = pmsg.Lives;

            if (pmsg.Score > this.Score)
                this.Score = pmsg.Score;
        }
    }
}