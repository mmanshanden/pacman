using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class ModelLibrary
    {
        private GraphicsDevice graphicsDevice;

        Dictionary<string, ModelBuilder> models;

        private ModelBuilder activeModel;

        public ModelLibrary(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;

            this.models = new Dictionary<string, ModelBuilder>();

            ModelBuilder block = this.BeginModel();
            block.PrimitiveBatch.SetColor(Color.Black);
            block.PrimitiveBatch.DrawCube();
            this.EndModel("block");
        }

        public ModelBuilder BeginModel()
        {
            this.activeModel = new ModelBuilder();
            return this.activeModel;
        }

        public void EndModel(string name)
        {
            if (this.activeModel == null)
                throw new Exception("No begin model set");

            this.activeModel.Save(graphicsDevice);
            this.models[name] = this.activeModel;
            this.activeModel = null;
        }

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
