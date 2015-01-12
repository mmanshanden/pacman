using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public class GameWorld : GameObjectList
    {
        public GameBoard GameBoard
        {
            get;
            private set;
        }

        public List<GameCharacter> gameCharacters;
        
        public GameWorld()
        {
            this.gameCharacters = new List<GameCharacter>();
        }

        public void Add(GameCharacter gameCharacter)
        {
            this.gameCharacters.Add(gameCharacter);
            this.Add(gameCharacter as GameObject);
        }
        public void Add(GameBoard gameBoard)
        {
            this.GameBoard = gameBoard;
            this.Add(gameBoard as GameObject);
        }

        public override void Update(float dt)
        {
            foreach (GameCharacter character in this.gameCharacters)
            {
                foreach (GameObject gameObject in this.gameObjects)
                {
                    if (character == gameObject)
                        continue;

                    if (character.CollidesWith(gameObject))
                    {
                        character.Collision_GameObject(gameObject);
                        gameObject.Collision_GameObject(character);
                    }
                }

                character.Move(dt);
            }

            base.Update(dt);
        }

        public override void Draw(DrawHelper drawHelper)
        {
            base.Draw(drawHelper);
        }
    }
}
