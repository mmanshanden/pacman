using _3dgl;
using Microsoft.Xna.Framework;
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

                if (this.GameBoard.Size == null)
                    continue;

                // gameobject leaving right side
                if (character.Position.X > this.GameBoard.Size.X)
                    character.Position -= Vector2.UnitX * this.GameBoard.Size.X;

                // ... leaving left side
                if (character.Position.X < -1)
                    character.Position += Vector2.UnitX * this.GameBoard.Size.X;
                
                // ... top side
                if (character.Position.Y > this.GameBoard.Size.Y)
                    character.Position -= Vector2.UnitY * this.GameBoard.Size.Y;

                // and bottom
                if (character.Position.Y < -1)
                    character.Position += Vector2.UnitY * this.GameBoard.Size.Y;
                
            }

            base.Update(dt);
        }

        public override void Draw(DrawManager drawManager)
        {
            base.Draw(drawManager);
        }
    }
}
