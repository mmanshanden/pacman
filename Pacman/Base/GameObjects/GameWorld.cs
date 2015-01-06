using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameWorld : GameObjectList
    {
        protected GameBoard gameBoard;
        
        public GameWorld()
        {

        }

        public override void Update(float dt)
        {
            foreach (GameObject gameObject in base.gameObjects)
            {
                foreach (GameObject other in this.gameObjects)
                {
                    if (gameObject == other)
                        continue;

                    if (gameObject.CollidesWith(other))
                    {
                        gameObject.Collision_GameObject(other);
                        other.Collision_GameObject(gameObject);
                    }
                }
            }
        }

        public override void Draw(DrawHelper drawHelper)
        {
            this.gameBoard.Draw(drawHelper);

            base.Draw(drawHelper);
        }
    }
}
