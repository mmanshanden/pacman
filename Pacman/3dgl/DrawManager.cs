using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    /// <summary>
    /// Builds and contains the structure of object
    /// needed for drawing 3d primitives and models
    /// </summary>
    public class DrawManager
    {
        GraphicsDevice graphicsDevice;
        BasicEffect effect; // shader provided by xna

        /// <summary>
        /// Main library
        /// </summary>
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

        /// <summary>
        /// Initializes the basic shader effect properties. (Lighting and view
        /// matrix).
        /// </summary>
        public void Initialize()
        {
            this.effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                this.graphicsDevice.Viewport.AspectRatio,
                0.1f,
                100f                   
            );

            this.effect.EnableDefaultLighting();
            this.effect.VertexColorEnabled = true; // use the color saved in vertices.
        }

        /// <summary>
        /// Restores the graphicsdevice to a state necessary for drawing. 
        /// </summary>
        public void BeginDraw()
        {
            // undo graphicsdevice changes done by spritebatch
            this.graphicsDevice.BlendState = BlendState.Opaque;
            this.graphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            this.graphicsDevice.RasterizerState = RasterizerState.CullNone;
        }

        /// <summary>
        /// Draw model with known graphicsdevice and basic shader effect
        /// provided by XNA.
        /// </summary>
        /// <param name="name">Name of model as saved in main library</param>
        public void DrawModel(string name)
        {
            this.ModelLibrary.DrawModel(name, this.effect);
        }

        #region Transformations
        // Following transformations translates given 2d x and y
        // values to 3d space.

        public void Translate(float x, float y)
        {
            this.effect.World *= Matrix.CreateTranslation(x, 0, y);
        }
        
        public void Scale(float x, float y)
        {
            this.effect.World *= Matrix.CreateScale(x, 1, y);
        }

        /// <summary>
        /// Perform rotation over y-axis (axis going up).
        /// </summary>
        /// <param name="radians">Angle in radians</param>
        public void Rotate(float radians)
        {
            this.effect.World *= Matrix.CreateRotationY(radians);
        }

        /// <summary>
        /// Translates, then rotates and finally translates back.
        /// </summary>
        /// <param name="radians"></param>
        /// <param name="point"></param>
        public void RotateOver(float radians, Vector2 point)
        {
            this.Translate(-point);
            this.Rotate(radians);
            this.Translate(point);
        }
        #endregion

        #region Vector overloads
        // same methods but now accepting a vector2.
        public void Translate(Vector2 translation)
        {
            this.Translate(translation.X, translation.Y);
        }
        public void Scale(Vector2 vector)
        {
            this.Scale(vector.X, vector.Y);
        }
        #endregion
    }
}
