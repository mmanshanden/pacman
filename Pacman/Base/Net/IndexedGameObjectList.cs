using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace Base
{
    public class IndexedGameObjectList : GameObject
    {
        Dictionary<int, GameObject> gameObjects;
        Dictionary<int, NetMessageContent> messages;

        public IndexedGameObjectList()
        {
            this.gameObjects = new Dictionary<int, GameObject>();
            this.messages = new Dictionary<int, NetMessageContent>();
        }

        public void Add(int id, GameObject gameObject)
        {
            this.gameObjects[id] = gameObject;
            this.messages[id] = new NetMessageContent();
        }

        public bool Contains(int id)
        {
            return this.gameObjects.ContainsKey(id);
        }


        public void UpdateObject(int id, NetMessageContent cmsg)
        {
            this.messages[id] = cmsg;
            this.gameObjects[id].UpdateObject(cmsg);
        }
        public void UpdateMessage(int id, NetMessageContent cmsg)
        {
            this.gameObjects[id].UpdateMessage(cmsg);
            this.messages[id] = cmsg;
        }

        public void WriteAllToMessage(NetMessage msg)
        {
            foreach (NetMessageContent cmsg in this.messages.Values)
                msg.SetData(cmsg);
        }
    }
}
