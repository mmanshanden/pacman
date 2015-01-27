using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Base
{
    /// <summary>
    /// Draw 2d textures on screen. Positions are relative, meaning
    /// that (1, 1) is bottom right corner of the screen and (0, 0)
    /// is top left corner of the screen. Screen property must be
    /// set to resolution of screen.
    /// </summary>
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
        
        // set to resolution
        public Vector2 Screen { get; set; }
        // used for drawing
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

        // loads textures
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
            this.textures["menu_pause_resume"]   = content.Load<Texture2D>("sprites/menu/pause/resume");
            this.textures["menu_pause_mainmenu"] = content.Load<Texture2D>("sprites/menu/pause/mainmenu"); 

            // lobby
            this.textures["menu_lobby_frame"]  = content.Load<Texture2D>("sprites/menu/lobbyhost/frame");
            this.textures["menu_lobby_you"]    = content.Load<Texture2D>("sprites/menu/lobbyhost/you");
            this.textures["menu_lobby_partner"] = content.Load<Texture2D>("sprites/menu/lobbyhost/partner");

            this.spriteFont    = content.Load<SpriteFont>("fonts/ui");
            this.spriteFontBig = content.Load<SpriteFont>("fonts/bigui");
        }

        /// <summary>
        /// Moves position over given size in a direction determined by origin enum value.
        /// </summary>
        /// <param name="position">Relative position on screen</param>
        /// <param name="size">Size of object</param>
        /// <param name="origin">Corner of object that should not be moved</param>
        /// <returns>New position to draw</returns>
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

        /// <summary>
        /// Draws texture on screen at relative position.
        /// </summary>
        public void DrawBox(string texture, Vector2 position, Origin origin)
        {
            position *= this.Screen;

            Texture2D t = this.textures[texture];
            Vector2 size = new Vector2(t.Width, t.Height);

            position = this.TranslatePosition(position, size, origin);

            this.spriteBatch.Draw(t, position, Color.White);
        }

        public void DrawBox(Texture2D texture, Vector2 position, Origin origin)
        {
            position *= this.Screen;
            Vector2 size = new Vector2(texture.Width, texture.Height);

            position = this.TranslatePosition(position, size, origin);
            this.spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Draws a box filled with given color at relative position and of relative size
        /// </summary>
        /// <param name="color"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public void DrawOverlay(Color color, Vector2 position, Vector2 size)
        {
            position *= this.Screen;
            size *= this.Screen; 

            Rectangle overlay = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.spriteBatch.Draw(this.textures["pixel"], overlay, color);
        }

        /// <summary>
        /// Draws string on screen at relative position.
        /// </summary>
        public void DrawString(string text, Vector2 position, Origin origin, Color color)
        {
            this.DrawString(this.spriteFont, text, position, origin, color);
        }

        /// <summary>
        /// Draws big string on screen at relative position.
        /// </summary>
        public void DrawStringBig(string text, Vector2 position, Origin origin, Color color)
        {
            this.DrawString(this.spriteFontBig, text, position, origin, color);
        }

        /// <summary>
        /// Draws string of given spritefont such that it will be aligned to
        /// at given position and origin.
        /// </summary>
        private void DrawString(SpriteFont spriteFont, string text, Vector2 position, Origin origin, Color color)
        {
            position *= this.Screen;
            Vector2 size = spriteFont.MeasureString(text);

            // update position to origin and screen size
            position = this.TranslatePosition(position, size, origin);

            this.spriteBatch.DrawString(spriteFont, text, position, color);
        }
    }
}
