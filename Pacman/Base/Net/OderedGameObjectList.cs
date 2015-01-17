using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network;

namespace Base.Net
{
    /// <summary>
    /// Stores gameobjects and updates their netdata in 
    /// the same order they were added.
    /// </summary>
    class OderedGameObjectList : GameObject
    {
        
        private int index;
        private List<GameObject> gameObjects;

        public OderedGameObjectList()
        {
            this.index = 0;
            this.gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void OverwriteGameObject(NetMessage netMessage)
        {
            // do stuff

            this.index++;
        }

        public NetMessage CopyToMessage()
        {
            // do stuff

            this.index++;
            return new NetMessage();
        }

        public void Restart()
        {
            this.index = 0;
        }

        public override void Update(float dt)
        {
            this.index = 0;
        }
    }
}
