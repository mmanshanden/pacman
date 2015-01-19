using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameObjectList : GameObject
    {
        protected List<GameObject> gameObjects, toRemove;

        public GameObjectList()
        {
            this.gameObjects = new List<GameObject>();
            this.toRemove = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
            gameObject.Parent = this;
        }

        public void Remove(GameObject gameObject)
        {
            this.toRemove.Add(gameObject); 
        }

        public void Clear()
        {
            this.toRemove.Clear();
            this.gameObjects.Clear();
        }

        public override void Update(float dt)
        {
            foreach (GameObject gameobject in toRemove)
                this.gameObjects.Remove(gameobject);
            

            foreach (GameObject gameObject in this.gameObjects)
                gameObject.Update(dt);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            foreach (GameObject gameObject in this.gameObjects)
                gameObject.Draw(drawHelper);
        }
    }
}
