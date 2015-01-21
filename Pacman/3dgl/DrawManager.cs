using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    /*
     * IMPORTANT:
     * http://xboxforums.create.msdn.com/forums/t/1796.aspx
     * 
     * 
     * 
     */
    public class DrawManager
    {
        GraphicsDevice graphicsDevice;
        BasicEffect effect;

        public ModelLibrary ModelLibrary
        {
            get;
            private set;
        }

        public Camera Camera
        {
            get;
            private set;
        }

        public DrawManager(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.effect = new BasicEffect(graphicsDevice);
            this.ModelLibrary = new ModelLibrary(graphicsDevice, content);
            this.Camera = new Camera(this.effect);
        }

        public void Initialize()
        {
            this.effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                this.graphicsDevice.Viewport.AspectRatio,
                0.1f,
                100f                   
            );

            this.effect.EnableDefaultLighting();
            this.effect.VertexColorEnabled = true;
        }

        public void BeginDraw()
        {
            // undo graphicsdevice changes done by spritebatch
            this.graphicsDevice.BlendState = BlendState.Opaque;
            this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            this.graphicsDevice.RasterizerState = RasterizerState.CullNone;
        }

        public void DrawModel(string name)
        {
            this.ModelLibrary.DrawModel(name, this.effect);
        }

        public void Translate(float x, float y)
        {
            this.effect.World *= Matrix.CreateTranslation(x, 0, y);
        }
        public void Translate(Vector2 translation)
        {
            this.Translate(translation.X, translation.Y);
        }
        public void Scale(float x, float y)
        {
            this.effect.World *= Matrix.CreateScale(x, 1, y);
        }
        public void Scale(Vector2 vector)
        {
            this.Scale(vector.X, vector.Y);
        }

        public void Rotate(float radians)
        {
            this.effect.World *= Matrix.CreateRotationY(radians);
        }
        public void RotateOver(float radians, Vector2 point)
        {
            this.Translate(-point);
            this.Rotate(radians);
            this.Translate(point);
        }
    }
}
