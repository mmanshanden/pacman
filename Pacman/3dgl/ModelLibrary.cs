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
    /// A library for all models.
    /// It contains models and a graphicsdevice needed
    /// for drawing the models.
    /// </summary>
    public class ModelLibrary
    {
        private GraphicsDevice graphicsDevice;
        private ContentManager content;

        // save models by name
        Dictionary<string, ModelBuilder> models;

        private ModelBuilder activeModel;

        /// <summary>
        /// Creates a new ModelLibrary
        /// </summary>
        /// <param name="graphicsDevice">Graphicsdevice used for drawing the models</param>
        /// <param name="content">Contentmanager provided to modelbuilders</param>
        public ModelLibrary(GraphicsDevice graphicsDevice, ContentManager content)
        {
            this.graphicsDevice = graphicsDevice;
            this.content = content;

            this.models = new Dictionary<string, ModelBuilder>();

            // immediately add a simple block/cube model
            ModelBuilder block = this.BeginModel();
            block.PrimitiveBatch.SetColor(Color.White);
            block.PrimitiveBatch.DrawCube();
            this.EndModel("block");
        }

        /// <summary>
        /// Sets a new working model.
        /// </summary>
        /// <returns>ModelBuilder object for building the 3d model</returns>
        public ModelBuilder BeginModel()
        {
            this.activeModel = new ModelBuilder(this.content);
            return this.activeModel;
        }

        /// <summary>
        /// Check whether provided name already exists
        /// </summary>
        /// <param name="name">Name of the model</param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            return this.models.ContainsKey(name);
        }

        /// <summary>
        /// Saves the current working model (see BeginModel() ). 
        /// </summary>
        /// <param name="name">Name of model</param>
        public void EndModel(string name)
        {
            if (this.activeModel == null)
                throw new Exception("No begin model set");

            this.activeModel.Save(graphicsDevice);
            this.models[name] = this.activeModel;
            this.activeModel = null;
        }
        
        /// <summary>
        /// Draws the model with provided shader effect.
        /// </summary>
        /// <param name="name">Name of model</param>
        /// <param name="effect">Shader</param>
        public void DrawModel(string name, Effect effect)
        {
            ModelBuilder model = this.models[name];
            model.SetToDevice(this.graphicsDevice);

            effect.CurrentTechnique.Passes[0].Apply();
            this.graphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList,
                0,
                0,
                model.VertexCount,
                0,
                model.PrimitiveCount
            );
        }
    }
}
