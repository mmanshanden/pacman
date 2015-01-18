using Base;
using Network;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pacman : GameCharacter
    {
        public Pacman()
        {

        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Yellow);
            drawHelper.Translate(-this.Position);
        }


        public override NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            PlayerMessage pmsg = new PlayerMessage();
            NetMessageContent.CopyOver(cmsg, pmsg);
            
            pmsg.Position = this.Position;
            pmsg.Direction = this.Direction;
            pmsg.Speed = this.Speed;
            
            return pmsg;
        }

        public override void UpdateObject(NetMessageContent cmsg)
        {
            PlayerMessage pmsg = (PlayerMessage)cmsg;

            this.Position = pmsg.Position;
            this.Direction = pmsg.Direction;
            this.Speed = pmsg.Speed;
        }
    }
}