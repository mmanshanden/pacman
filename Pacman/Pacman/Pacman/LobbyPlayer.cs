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

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.DrawString(this.Name, this.Position, DrawHelper.Origin.TopLeft, Color.White);
        }
    }
}
