﻿using Base;
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
        private int ghostCombo;
        
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
            this.ghostCombo = 1; 

            this.time = AnimationTime;
            this.closed = false;
        }

        public override void Collision_GameObject(GameObject gameObject)
        {
            Ghost ghost = gameObject as Ghost;
            if (ghost != null)
            {
                if (ghost.State == Ghost.States.Chase || ghost.State == Ghost.States.Scatter || ghost.State == Ghost.States.Leave)
                    this.Die();
                if (ghost.State == Ghost.States.Frightened)
                {
                    int ghostScore = 10 * this.ghostCombo;
                    this.Score = this.Score + ghostScore;
                    this.ghostCombo *= 2;
                }
            }

            else if (gameObject is Bubble)
            {
                this.Score++;
            }

            else if (gameObject is Powerup)
            {
                this.ghostCombo = 1; 
                Level level = (Level)this.Parent;

                foreach (GhostHouse ghostHouse in level.GhostHouses)
                    ghostHouse.FrightenGhosts();
                
                Game.SoundManager.PlaySoundEffect("powerup");
            }
        }

        public virtual void Die()
        {
            this.Lives--;
            this.Position = this.spawn;
            Level level = (Level)this.Parent;

            foreach (GhostHouse ghostHouse in level.GhostHouses)
                ghostHouse.ResetGhosts();

            if (this.Lives > 0)
                Game.SoundManager.PlaySoundEffect("live_lost");
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

        public static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.8f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            mb.BuildFromTexture("voxels/pacmanopen", 16);
            modelLibrary.EndModel("pacman_open");
            
            
            mb = modelLibrary.BeginModel();

            mb.PrimitiveBatch.Translate(Vector3.One * 0.5f);
            mb.PrimitiveBatch.Scale(Vector3.One * 1.8f);
            mb.PrimitiveBatch.Translate(Vector3.One * -0.5f);

            mb.BuildFromTexture("voxels/pacmanclosed", 16);
            modelLibrary.EndModel("pacman_closed");
        }

        public override void Draw(DrawManager drawManager)
        {
            if (this.Lives < 1)
                return;
            
            if (this.Velocity != Vector2.Zero)
                this.rotation = (float)System.Math.Atan2(this.Direction.X, this.Direction.Y);

            drawManager.RotateOver(this.rotation, Vector2.One * 0.5f);
            drawManager.Translate(this.Position.X, this.Position.Y);

            if (this.closed)
                drawManager.DrawModel("pacman_closed");
            else
                drawManager.DrawModel("pacman_open");

            drawManager.Translate(-this.Position.X, -this.Position.Y);
            drawManager.RotateOver(-rotation, Vector2.One * 0.5f);
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

            if (pmsg.Lives != -1)
                this.Lives = pmsg.Lives;

            if (pmsg.Score != -1)
                this.Score = pmsg.Score;
        }
    }
}