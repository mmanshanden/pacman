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

        public IndexedGameObjectList()
        {
            this.gameObjects = new Dictionary<int, GameObject>();
        }

        public void Add(int id, GameObject gameObject)
        {
            this.gameObjects[id] = gameObject;
        }

        public bool Contains(int id)
        {
            return this.gameObjects.ContainsKey(id);
        }


        public void UpdateObject(int id, NetMessageContent cmsg)
        {
            this.gameObjects[id].UpdateObject(cmsg);
        }
        public void UpdateMessage(int id, NetMessageContent cmsg)
        {
            this.gameObjects[id].UpdateMessage(cmsg);
        }
        
        public void WriteAllToMessage(NetMessage msg, NetMessageContent baseMessage)
        {
            foreach(KeyValuePair<int, GameObject> entry in this.gameObjects)
            {
                NetMessageContent cmsg = entry.Value.UpdateMessage(baseMessage);
                cmsg.Id = entry.Key;

                msg.SetData(cmsg);
            }
        }
    }
}
