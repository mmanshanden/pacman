using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace Base
{
    class IndexedGameObjectList : GameObject
    {
        Dictionary<int, GameObject> gameObjects;

        public IndexedGameObjectList()
        {

        }

        public void Add(int id, GameObject gameObject)
        {
            this.gameObjects[id] = gameObject;
        }

        public bool Contains(int id)
        {
            return this.gameObjects.ContainsKey(id);
        }

        public void OverwriteGameObject(int id, NetMessage message)
        {
            // do stuff
        }
        public NetMessage CopyToMessage(int id)
        {
            // do stuff

            return new NetMessage();
        }
    }
}
