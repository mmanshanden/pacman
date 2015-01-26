using Base;
using Microsoft.Xna.Framework;
using Network;

namespace Pacman
{
    class LobbyPlayer : GameObject
    {
        public readonly string[] names = new string[] 
        {
            "Peaceful Pacman",
            "Perfect Pacman",
            "Passionate Pacman",
            "Infamous Inky",
            "Immortal Inky",
            "Incredible Inky",
            "Illogical Inky",
            "Cataclysmic Clyde"
        };
      
        public int Score
        {
            get;
            set; 
        }

        public string Name
        {
            get { return this.names[this.name]; }
        }

        private int name;
        private float rotation;

        public LobbyPlayer()
        {
            this.name = Game.Random.Next(this.names.Length);
        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                this.rotation -= 0.05f;

            if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                this.rotation += 0.05f;
        }

        // Set data for outgoing message
        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            LobbyMessage lmsg = new LobbyMessage();
            NetMessageContent.CopyOver(cmsg, lmsg);

            lmsg.Score = this.Score;
            lmsg.Rotation = this.rotation;
            lmsg.Name = this.name;

            return lmsg;
        }

        // Updates gameObjects from received message
        public override void UpdateObject(NetMessageContent cmsg)
        {
            LobbyMessage lmsg = (LobbyMessage)cmsg;

            if (lmsg.Score != -1)
                this.Score = lmsg.Score;

            this.name = lmsg.Name;
            this.rotation = lmsg.Rotation;
        }

        public override void Draw(_3dgl.DrawManager drawManager)
        {
            drawManager.Translate(-0.5f, -0.5f);
            drawManager.RotateX(-MathHelper.PiOver2);
            drawManager.Translate(0.5f, 0.5f);

            drawManager.Translate(-0.5f, -0.5f);
            drawManager.RotateY(this.rotation);
            drawManager.Translate(0.5f, 0.5f);

            drawManager.Translate(-0.5f, 5);
            drawManager.Scale(0.05f, 0.05f);
            drawManager.Translate(this.Position);
            drawManager.DrawModel("pacman_open");
            drawManager.Translate(-this.Position);
            drawManager.Scale(20f, 20f);
            drawManager.Translate(0.5f, -5);
            
            drawManager.Translate(-0.5f, -0.5f);
            drawManager.RotateY(-this.rotation);
            drawManager.Translate(0.5f, 0.5f);

            drawManager.Translate(-0.5f, -0.5f);
            drawManager.RotateX(MathHelper.PiOver2);
            drawManager.Translate(0.5f, 0.5f);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.DrawString(this.Name, this.Position, DrawHelper.Origin.Center, Color.White);
        }
    }
}
