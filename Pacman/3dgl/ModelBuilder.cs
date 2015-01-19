using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class ModelBuilder
    {
        private ContentManager content;

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

        public ModelBuilder(ContentManager content)
        {
            this.content = content;
            this.PrimitiveBatch = new PrimitiveBatch();

            this.saved = false;
        }

        public void BuildFromTexture(string asset, int layers)
        {
            Texture2D voxeltexture = this.content.Load<Texture2D>(asset);

            int layer_width = voxeltexture.Width / layers;

            int lx = layer_width;
            int ly = voxeltexture.Height;
            int lz = layers;

            Vector3 scale = new Vector3(1f / lx, 1f / ly, 1f / lz);
            this.PrimitiveBatch.Scale(scale);

            Rectangle layerbox = new Rectangle();
            layerbox.Width = layer_width;
            layerbox.Height = voxeltexture.Height;

            for (int z = 0; z < layers; z++)
            {                
                // move layerbox over layer nr
                layerbox.X = z * layerbox.Width;

                Color[] layerdata = new Color[layerbox.Width * layerbox.Height];
                voxeltexture.GetData(0, layerbox, layerdata, 0, layerdata.Length);

                for (int x = 0; x < layerbox.Width; x++)
                {
                    for (int y = 0; y < layerbox.Height; y++)
                    {
                        this.PrimitiveBatch.Translate(new Vector3(x, y, z));
                        
                        Color voxel = layerdata[x + y * layerbox.Width];
                        
                        if (voxel.A > 50)
                        {
                            this.PrimitiveBatch.SetColor(voxel);
                            this.PrimitiveBatch.DrawCube();
                        }

                        this.PrimitiveBatch.Translate(new Vector3(-x, -y, -z));
                    }
                }
            }

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
