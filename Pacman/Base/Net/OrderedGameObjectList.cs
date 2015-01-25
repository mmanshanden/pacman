using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace Base
{
    /// <summary>
    /// Stores gameobjects and updates their netdata in 
    /// the same order they were added.
    /// </summary>
    public class OrderedGameObjectList : GameObject
    {
        
        private int index;
        private List<GameObject> gameObjects;

        public int Count
        {
            get { return this.gameObjects.Count; }
        }

        public OrderedGameObjectList()
        {
            this.index = 0;
            this.gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public override void UpdateObject(NetMessageContent cmsg)
        {
            this.gameObjects[this.index].UpdateObject(cmsg);
            this.index++;

            if (index == this.Count)
            {
                index -= this.Count;
            }
        }

        public void Restart()
        {
            this.index = 0;
        }
        
        /// <summary>
        /// Updates the baseMessage for every game object in
        /// the collection. Then adds updates message content
        /// to the netmessage container.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="baseMessage"></param>
        public void WriteAllToMessage(NetMessage msg, NetMessageContent baseMessage)
        {
            for (int i = 0; i < this.Count; i++)
                msg.SetData(this.gameObjects[i].UpdateMessage(baseMessage));
        }

    }
}
