using _3dgl;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    /// <summary>
    /// Contains gameobjects and gamecharacters.
    /// Checks whether gamecharacters collide
    /// with gameobjects. 
    /// </summary>
    public class GameWorld : GameObjectList
    {
        public GameBoard GameBoard
        {
            get;
            private set;
        }

        public readonly Vector2 VoidPoint = new Vector2(-20, -20);

        public List<GameCharacter> gameCharacters;
        
        public GameWorld()
        {
            this.gameCharacters = new List<GameCharacter>();
        }

        public void Add(GameCharacter gameCharacter)
        {
            this.gameCharacters.Add(gameCharacter);
            base.Add(gameCharacter);
        }
        public void Add(GameBoard gameBoard)
        {
            this.GameBoard = gameBoard;
            base.Add(gameBoard);
        }

        public override void Update(float dt)
        {
            foreach (GameCharacter character in this.gameCharacters)
            {
                foreach (GameObject obstacle in this.gameObjects)
                {
                    // dont collide with self
                    if (character == obstacle)
                        continue;

                    if (character.CollidesWith(obstacle))
                    {
                        // call collision event for both
                        character.Collision_GameObject(obstacle);
                        obstacle.Collision_GameObject(character);
                    }
                }

                if (this.GameBoard.Size == null)
                    continue;


                if (character.Position == this.VoidPoint)
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

    }
}
