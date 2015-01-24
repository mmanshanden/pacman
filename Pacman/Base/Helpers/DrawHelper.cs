using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Base
{
    public class DrawHelper
    {
        public enum Origin
        {
            TopLeft,
            TopRight,
            Center,
            BottomLeft,
            BottomRight,
        }
        
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont spriteFontBig;

        private Dictionary<string, Texture2D> textures;
        
        public Vector2 Screen { get; set; }     
        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
        }

        public DrawHelper(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.textures = new Dictionary<string, Texture2D>();
        }

        public void LoadTextures(ContentManager content)
        {
            Texture2D pixel = new Texture2D(this.graphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            this.textures["pixel"] = pixel;
            
            // controls
            this.textures["menu_controls_ps_back"]          = content.Load<Texture2D>("sprites/menu/back/playstation");
            this.textures["menu_controls_kb_back"]          = content.Load<Texture2D>("sprites/menu/back/keyboard");
            this.textures["menu_controls_ps_serverbrowser"] = content.Load<Texture2D>("sprites/menu/serverbrowser/playstation");
            this.textures["menu_controls_kb_serverbrowser"] = content.Load<Texture2D>("sprites/menu/serverbrowser/keyboard");
            this.textures["menu_controls_ps_gamemode"]      = content.Load<Texture2D>("sprites/menu/gamemode/playstation");
            this.textures["menu_controls_kb_gamemode"]      = content.Load<Texture2D>("sprites/menu/gamemode/keyboard");
            this.textures["menu_controls_ps_lobbyhost"]     = content.Load<Texture2D>("sprites/menu/lobbyhost/playstation");
            this.textures["menu_controls_kb_lobbyhost"]     = content.Load<Texture2D>("sprites/menu/lobbyhost/keyboard");

            // gamemode selection menu
            this.textures["menu_gamemode_singleplayer"] = content.Load<Texture2D>("sprites/menu/gamemode/singleplayer");
            this.textures["menu_gamemode_multiplayer"]  = content.Load<Texture2D>("sprites/menu/gamemode/multiplayer");

            //pause
            this.textures["PauseOverlay"] = content.Load<Texture2D>("sprites/menu/pause/pausebackground");
            this.textures["PauseResume"] = content.Load<Texture2D>("sprites/menu/pause/pauseresume");
            this.textures["PauseMainMenu"] = content.Load<Texture2D>("sprites/menu/pause/pausemainmenu"); 

            this.spriteFont    = content.Load<SpriteFont>("fonts/ui");
            this.spriteFontBig = content.Load<SpriteFont>("fonts/bigui");
        }

        private Vector2 TranslatePosition(Vector2 position, Vector2 size, Origin origin)
        {
            switch (origin)
            {
                case Origin.TopRight:
                    position.X -= size.X;
                    break;

                case Origin.Center:
                    position -= size / 2;
                    break;

                case Origin.BottomLeft:
                    position.Y -= size.Y;
                    break;

                case Origin.BottomRight:
                    position -= size;
                    break;
            }

            return position;
        }

        public void DrawBox(string texture, Vector2 position, Origin origin)
        {
            position *= this.Screen;

            Texture2D t = this.textures[texture];
            Vector2 size = new Vector2(t.Width, t.Height);

            position = this.TranslatePosition(position, size, origin);

            this.spriteBatch.Draw(t, position, Color.White);
        }

        public void DrawOverlay(Color color, Vector2 position, Vector2 size)
        {
            position *= this.Screen;
            size *= this.Screen; 

            Rectangle overlay = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.spriteBatch.Draw(this.textures["pixel"], overlay, color);
        }

        public void DrawString(string text, Vector2 position, Origin origin, Color color)
        {
            this.DrawString(this.spriteFont, text, position, origin, color);
        }
        public void DrawStringBig(string text, Vector2 position, Origin origin, Color color)
        {
            this.DrawString(this.spriteFontBig, text, position, origin, color);
        }

        private void DrawString(SpriteFont spriteFont, string text, Vector2 position, Origin origin, Color color)
        {
            position *= this.Screen;
            Vector2 size = spriteFont.MeasureString(text);

            position = this.TranslatePosition(position, size, origin);

            this.spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}
