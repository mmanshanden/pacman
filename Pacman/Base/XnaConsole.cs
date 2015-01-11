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
        List<string> TextLines;
        SpriteFont Font;
        Texture2D consoleBackground;
        public bool visible { get; set; }

        public XnaConsole()
        {
            this.TextLines = new List<string>();
            this.visible = false; 
        }



        public void LoadContent(ContentManager content, Texture2D pixel)
        {
            this.Font = content.Load<SpriteFont>("Font");
            this.consoleBackground = pixel; 
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

            spriteBatch.Draw(consoleBackground, Vector2.Zero, Color.ForestGreen*0.5f);

            int c = TextLines.Count(); 

            for(int x = c; x > (c-10); x--)
            {
                // Y-position probably has to be changed
                spriteBatch.DrawString(Font, TextLines[x], new Vector2(15, 200-x*15), Color.Black);
            }


        }
    }
}
