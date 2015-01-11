using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Base
{
    public class DrawHelper
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;

        private Vector2 translations;
        private Vector2 scale;

        private Texture2D pixel;

        public static Texture2D Pixel
        {
            get;
            private set;
        }

        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
        }

        public DrawHelper(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);

            this.translations = Vector2.Zero;
            this.scale = Vector2.One;
        }

        public void LoadTextures(ContentManager content)
        {
            this.pixel = new Texture2D(this.graphicsDevice, 1, 1);
            this.pixel.SetData(new Color[] { Color.White });
            DrawHelper.Pixel = this.pixel;
        }

        #region Transformations
        public void Translate(Vector2 translation)
        {
            this.translations += translation * this.scale;
        }
        public void Translate(float x, float y)
        {
            this.Translate(new Vector2(x, y));
        }
        public void Scale(Vector2 scale)
        {
            this.scale *= scale;
        }
        public void Scale(float x, float y)
        {
            this.Scale(new Vector2(x, y));
        }
        #endregion

        public void DrawBox(Color color)
        {
            Rectangle r = new Rectangle();
            r.X = (int)this.translations.X;
            r.Y = (int)this.translations.Y;
            r.Width = (int)this.scale.X;
            r.Height = (int)this.scale.Y;

            this.spriteBatch.Draw(this.pixel, r, color);
        }
    }
}
