using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Base
{
    public class XnaConsole
    {
        List<string> textLines;
        SpriteFont font;
        Texture2D consoleBackground;
        public bool Visible { get; set; }

        public XnaConsole()
        {
            this.textLines = new List<string>();
            this.Visible = true;

            this.WriteLine("Console loaded");
            this.WriteLine("-----------------------------------");
        }
        
        public void LoadContent(ContentManager content, Texture2D pixel)
        {
            this.font = content.Load<SpriteFont>("Font");
            this.consoleBackground = pixel; 
        }

        public void WriteLine(string line) 
        {
            if (textLines.Count == 10)
            {
                textLines.RemoveAt(0); 
            }

            textLines.Add(line); 
        }

        public void Clear()
        {
            textLines.Clear(); 
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            if (!this.Visible)
                return;

            spriteBatch.Draw(
                consoleBackground, 
                new Rectangle(0, 0, (int)screenSize.X, (int)screenSize.Y / 2),
                Color.Black * 0.5f
            );

            int c = textLines.Count();

            Vector2 position = new Vector2(5, 5);

            foreach (string line in this.textLines)
            {
                spriteBatch.DrawString(font, line, position, Color.White);
                position.Y += 12;
            }

        }
    }
}
