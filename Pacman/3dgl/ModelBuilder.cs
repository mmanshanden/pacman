using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class ModelBuilder
    {
        private GraphicsDevice graphicsDevice;
        private PrimitiveBatch primitiveBatch;

        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public ModelBuilder(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public PrimitiveBatch Begin()
        {
            this.primitiveBatch = new PrimitiveBatch();
            return this.primitiveBatch;
        }

        public void End(string name)
        {
            Vertex[] vertices = this.primitiveBatch.GetVertexBatch();
            short[] indices = this.primitiveBatch.GetIndexBatch();

            this.vertexBuffer = new VertexBuffer(
                this.graphicsDevice, 
                typeof(Vertex), 
                vertices.Length, 
                BufferUsage.None
            );

            this.vertexBuffer.SetData(vertices);

            this.indexBuffer = new IndexBuffer(
                this.graphicsDevice,
                typeof(short),
                indices.Length,
                BufferUsage.None
            );

            this.indexBuffer.SetData(indices);
            
            // gc primitive batch
            this.primitiveBatch = null;
        }



    }
}
