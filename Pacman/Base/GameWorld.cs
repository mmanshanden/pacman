using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameWorld
    {
        List<GameObject> gameObjects;

        public GameBoard GameBoard
        {
            get;
            set;
        }

        public GameWorld()
        {
            this.gameObjects = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
        {
            this.gameObjects.Add(gameObject);
        }

        public virtual void Update(float dt)
        {
            foreach (GameObject gameObject in this.gameObjects)
            {
                gameObject.Move(dt, this.GameBoard);
                gameObject.Update(dt);
            }
        }

        public virtual void Draw(DrawHelper drawHelper)
        {
            this.GameBoard.Draw(drawHelper);

            foreach (GameObject gameObject in this.gameObjects)
            {
                gameObject.Draw(drawHelper);
            }

        }
    }
}
