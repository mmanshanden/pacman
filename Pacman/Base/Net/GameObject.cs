using Network;

namespace Base
{
    public partial class GameObject
    {
        /// <summary>
        /// Updates the object variables based on the content message.
        /// </summary>
        public virtual void UpdateObject(NetMessageContent cmsg)
        {

        }

        /// <summary>
        /// Updates a content message based on object variables.
        /// </summary>
        public virtual NetMessageContent UpdateMessage(NetMessageContent cmsg)
        {
            return cmsg;
        }
    }
}
