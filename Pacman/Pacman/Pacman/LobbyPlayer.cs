using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Network;

namespace Pacman
{
    class LobbyPlayer : GameObject
    {      
        public int Score
        {
            get;
            set; 
        }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        private Texture2D model;
        private float rotation;
        private bool closed;

        public LobbyPlayer()
        {

        }

        public void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
                this.rotation += 0.05f;

            if (inputHelper.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
                this.rotation -= 0.05f;

            if (inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.W) ||
                inputHelper.KeyPressed(Microsoft.Xna.Framework.Input.Keys.S))
            {
                this.closed = !this.closed;
            }
        }

        // Set data for outgoing message
        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            LobbyMessage lmsg = new LobbyMessage();
            NetMessageContent.CopyOver(cmsg, lmsg);

            lmsg.Score = this.Score;
            lmsg.Rotation = this.rotation;
            lmsg.Closed = this.closed;

            return lmsg;
        }

        // Updates gameObjects from received message
        public override void UpdateObject(NetMessageContent cmsg)
        {
            LobbyMessage lmsg = (LobbyMessage)cmsg;

            if (lmsg.Score != -1)
                this.Score = lmsg.Score;

            this.rotation = lmsg.Rotation;
            this.closed = lmsg.Closed;
        }

        public override void Draw(_3dgl.DrawManager drawManager)
        {
            Game.Camera.Zoom = 2.5f;
            Game.Camera.Phi = 0;
            Game.Camera.Rho = 0.4f;
            Game.Camera.Target = Vector2.Zero;
            Game.Camera.SetCameraHeight(0.5f);

            if (this.model != null)
            {
                this.model.Dispose();
                this.model = null;
            }

            drawManager.RotateOver(this.rotation, Vector2.One * 0.5f);
            drawManager.Translate(-0.5f, -0.5f);

            string m = "pacman_open";

            if (this.closed)
                m = "pacman_closed";

            this.model = drawManager.DrawModelToTexture(m, 300, 300);
            drawManager.Translate(0.5f, 0.5f);
            drawManager.RotateOver(-this.rotation, Vector2.One * 0.5f);
            
        }

        public override void Draw(DrawHelper drawHelper)
        {
            if (this.Name == "You")
                drawHelper.DrawBox("menu_lobby_you", this.Position, DrawHelper.Origin.Center);
            else
                drawHelper.DrawBox("menu_lobby_partner", this.Position, DrawHelper.Origin.Center);

            drawHelper.DrawBox(this.model, this.Position + Vector2.UnitY * 0.24f, DrawHelper.Origin.Center);
            drawHelper.DrawBox("menu_lobby_frame", this.Position + Vector2.UnitY * 0.25f, DrawHelper.Origin.Center);

            if (this.Score != 0)
                drawHelper.DrawString(this.Score.ToString(), this.Position + Vector2.UnitY * 0.5f, DrawHelper.Origin.Center, Color.White);
        }
    }
}
