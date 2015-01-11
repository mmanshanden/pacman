using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Base
{
    class XnaConsole
    {
        List<string> TextLines;
        SpriteFont Font; 

        public XnaConsole()
        {
            this.TextLines = new List<string>(); 
        }



        public void LoadContent(ContentManager content)
        {
            this.Font = content.Load<SpriteFont>("Font");
        }

        public void WriteLine(string line) 
        {
            if (TextLines.Count == 10)
            {
                TextLines.RemoveAt(0); 
            }

            TextLines.Add(line); 
        }

        public void Clear()
        {
            TextLines.Clear(); 
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 screenSize)
        {
            int c = TextLines.Count(); 

            for(int x = c; x > (c-10); x--)
            {
                spriteBatch.DrawString(Font, TextLines[x], new Vector2(15, 200-x*15), Color.Black);
            }
        }
    }
}
