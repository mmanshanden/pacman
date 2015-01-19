using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class ModelBuilder
    {
        public PrimitiveBatch PrimitiveBatch
        {
            get;
            private set;
        }

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public int VertexCount { get; private set; }
        public int PrimitiveCount { get; private set; }

        private bool saved;

        public ModelBuilder()
        {
            this.PrimitiveBatch = new PrimitiveBatch();

            this.saved = false;
        }

        public void Save(GraphicsDevice graphicsDevice)
        {
            Vertex[] vertices = this.PrimitiveBatch.GetVertexBatch();
            short[] indices = this.PrimitiveBatch.GetIndexBatch();

            this.vertexBuffer = new VertexBuffer(
                graphicsDevice, 
                typeof(Vertex), 
                vertices.Length, 
                BufferUsage.None
            );

            this.vertexBuffer.SetData(vertices);
            this.VertexCount = vertices.Length;

            this.indexBuffer = new IndexBuffer(
                graphicsDevice,
                typeof(short),
                indices.Length,
                BufferUsage.None
            );

            this.indexBuffer.SetData(indices);
            this.PrimitiveCount = indices.Length / 3;
            this.saved = true;
            
            // garbage collect primitive batch
            this.PrimitiveBatch = null;
        }

        public void SetToDevice(GraphicsDevice device)
        {
            if (this.saved == false)
                throw new Exception("Model was not saved");

            device.SetVertexBuffer(this.vertexBuffer);
            device.Indices = this.indexBuffer;
        }



    }
}
