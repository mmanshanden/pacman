using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace Base
{
    /// <summary>
    /// Stores gameobject by id key. Updating
    /// the object with netdata requires the objects
    /// id.
    /// </summary>
    public class IndexedGameObjectList : GameObject
    {
        Dictionary<int, GameObject> gameObjects;

        public int Count
        {
            get { return this.gameObjects.Count; }
        }

        public IndexedGameObjectList()
        {
            this.gameObjects = new Dictionary<int, GameObject>();
        }

        public void Add(int id, GameObject gameObject)
        {
            this.gameObjects[id] = gameObject;
        }

        public void Remove(int id)
        {
            this.gameObjects.Remove(id);
        }

        public GameObject Get(int id)
        {
            return this.gameObjects[id];
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

        /// <summary>
        /// Updates the baseMessage for every game object in
        /// the collection. Then adds updates message content
        /// to the netmessage container.
        /// </summary>
        public void WriteAllToMessage(NetMessage msg, NetMessageContent baseMessage)
        {
            foreach(KeyValuePair<int, GameObject> entry in this.gameObjects)
            {
                NetMessageContent cmsg = entry.Value.UpdateMessage(baseMessage);
                cmsg.Id = entry.Key;

                msg.SetData(cmsg);
            }
        }

        public override void Draw(_3dgl.DrawManager drawManager)
        {
            foreach (GameObject gameObject in this.gameObjects.Values)
                gameObject.Draw(drawManager);
        }
        public override void Draw(DrawHelper drawHelper)
        {
            foreach (GameObject gameObject in this.gameObjects.Values)
                gameObject.Draw(drawHelper);
        }
        
    }
}
