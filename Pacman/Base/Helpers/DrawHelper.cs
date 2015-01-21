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
            Center,
            BottomRight,
        }
        
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;

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
            this.textures["menu_controls_kb_back"] = content.Load<Texture2D>("sprites/menu/back/keyboardback");
            this.textures["menu_controls_ps_serverbrowser"] = content.Load<Texture2D>("sprites/menu/serverbrowser/playstation");
            this.textures["menu_controls_kb_serverbrowser"] = content.Load<Texture2D>("sprites/menu/serverbrowser/keyboardserverbrowser");
            this.textures["menu_controls_ps_gamemode"]      = content.Load<Texture2D>("sprites/menu/gamemode/playstation");
            this.textures["menu_controls_kb_gamemode"] = content.Load<Texture2D>("sprites/menu/gamemode/keyboardselect");
            this.textures["menu_controls_ps_lobbyhost"]     = content.Load<Texture2D>("sprites/menu/lobbyhost/playstation");
            this.textures["menu_controls_kb_lobbyhost"] = content.Load<Texture2D>("sprites/menu/lobbyhost/keyboardlobbyhost");

            // gamemode selection menu
            this.textures["menu_gamemode_singleplayer"] = content.Load<Texture2D>("sprites/menu/gamemode/singleplayer");
            this.textures["menu_gamemode_multiplayer"]  = content.Load<Texture2D>("sprites/menu/gamemode/multiplayer");
        }

        public void DrawBox(string texture, Vector2 position, Origin origin)
        {
            position *= this.Screen;

            Texture2D t = this.textures[texture];
            Vector2 size = new Vector2(t.Width, t.Height);

            switch (origin)
            {
                case Origin.BottomRight:
                    position -= size;
                    break;

                case Origin.Center:
                    position -= size / 2;
                    break;
            }

            this.spriteBatch.Draw(t, position, Color.White);
        }
    }
}
