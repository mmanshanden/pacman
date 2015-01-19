using System;
using System.Collections.Generic;

namespace Base
{
    public class ObjectLoader
    {
        public List<GameObject> gameObjects;

        public ObjectLoader()
        {
            this.gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public void LoadObjects()
        {
            foreach (GameObject gameObject in this.gameObjects)
                gameObject.Load();

            this.gameObjects.Clear();
        }
    }
}
