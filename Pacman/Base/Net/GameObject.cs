using Network;

namespace Base
{
    public partial class GameObject
    {
        
        public virtual void UpdateObject(NetMessageContent cmsg)
        {

        }

        public virtual NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            return cmsg;
        }
    }
}
