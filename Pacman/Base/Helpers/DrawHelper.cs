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

        public SpriteBatch SpriteBatch
        {
            get { return this.spriteBatch; }
        }

        public DrawHelper(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public void LoadTextures(ContentManager content)
        {

        }
    }
}
