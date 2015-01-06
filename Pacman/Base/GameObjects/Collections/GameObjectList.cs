using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameObjectList : GameObject
    {
        protected List<GameObject> gameObjects;

        public GameObjectList()
        {
            this.gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
            gameObject.Parent = this;
        }

        public override void Draw(DrawHelper drawHelper)
        {
            foreach (GameObject gameObject in this.gameObjects)
                gameObject.Draw(drawHelper);
        }

        public override void Update(float dt)
        {
            foreach (GameObject gameObject in this.gameObjects)
                gameObject.Update(dt);
        }
    }
}
