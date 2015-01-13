using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Base
{
    public class Console
    {
        const int lineCount = 21;

        static GraphicsDevice graphicsDevice;
        static SpriteBatch spriteBatch;
        static Rectangle background;
        static SpriteFont consoleFont;
        static List<string> textLines;
        static Texture2D pixel;
        
        public static bool Visible { get; set; }

        public static void Initialize(GraphicsDevice graphics, ContentManager content)
        {
            graphicsDevice = graphics;
            spriteBatch = new SpriteBatch(graphics);
            textLines = new List<string>();

            background.Width = graphics.Viewport.Width;
            background.Height = graphics.Viewport.Height / 2;

            consoleFont = content.Load<SpriteFont>("font");

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            Visible = false;

            Console.WriteLine("Console loaded");
            Console.WriteLine("----------------------------");
        }

        public static void WriteLine(string line) 
        {
            if (textLines.Count == lineCount)
                textLines.RemoveAt(0);

            textLines.Add(line); 
        }

        public static void Clear()
        {
            textLines.Clear(); 
        }


        public static void Draw()
        {
            if (!Visible)
                return;

            spriteBatch.Begin();

            spriteBatch.Draw(pixel, background, Color.Black * 0.8f);

            Vector2 position = new Vector2(5, 0);

            foreach (string line in textLines)
            {
                spriteBatch.DrawString(consoleFont, line, position, Color.White);
                position.Y += 11;
            }

            spriteBatch.End();
        }
    }
}
