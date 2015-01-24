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
    /// Contains methods and properties for building 3d models. 
    /// These 3d models are saved on the graphics device through
    /// vertex- and indexbuffers. Vertex buffers allow for quicker
    /// draws while indexbuffers allow fewer vertices.
    /// </summary>
    public class ModelBuilder
    {
        private ContentManager content;

        // internal batch used for batching quads
        public PrimitiveBatch PrimitiveBatch
        {
            get;
            private set;
        }

        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public int VertexCount { get; private set; }
        public int PrimitiveCount { get; private set; }

        private bool saved; // finished building...

        public ModelBuilder(ContentManager content)
        {
            this.content = content; // for loading 2d textures on demand
            this.PrimitiveBatch = new PrimitiveBatch();

            this.saved = false;
        }

        /// <summary>
        /// Extracts the pixeldata from a Texture2D and
        /// uses it for building a 3d model. Each pixel
        /// is represented as a block (or cube) in 3d
        /// space. Having an "hollowed out" texture help
        /// saving vertices.
        /// </summary>
        /// <param name="asset">Texture2D file</param>
        /// <param name="layers">Number of layers contained in the Texture2D file</param>
        public void BuildFromTexture(string asset, int layers)
        {
            Texture2D voxeltexture = this.content.Load<Texture2D>(asset);

            int layer_width = voxeltexture.Width / layers;

            // dimensions
            int lx = layer_width;
            int ly = voxeltexture.Height;
            int lz = layers;

            // scale such that the end result will have 1x1x1 dimensions
            Vector3 scale = new Vector3(1f / lx, 1f / ly, 1f / lz);
            this.PrimitiveBatch.Scale(scale);

            // rectangle covering a single layer
            Rectangle layerbox = new Rectangle();
            layerbox.Width = layer_width;
            layerbox.Height = voxeltexture.Height;

            for (int z = 0; z < layers; z++)
            {                
                // move layerbox over layer nr
                layerbox.X = z * layerbox.Width;

                // fill color array
                Color[] layerdata = new Color[layerbox.Width * layerbox.Height];
                voxeltexture.GetData(0, layerbox, layerdata, 0, layerdata.Length);

                for (int x = 0; x < layerbox.Width; x++)
                {
                    for (int y = 0; y < layerbox.Height; y++)
                    {
                        this.PrimitiveBatch.Translate(new Vector3(x, y, z));
                        
                        // get the color of pixel x,y on layer z
                        Color voxel = layerdata[x + y * layerbox.Width];
                        
                        // if fully non-transparent
                        if (voxel.A == 255)
                        {
                            this.PrimitiveBatch.SetColor(voxel);
                            this.PrimitiveBatch.DrawCube();
                        }

                        this.PrimitiveBatch.Translate(new Vector3(-x, -y, -z));
                    }
                }
            }

        }

        /// <summary>
        /// Extracts the vertex and index data from the 
        /// primitive batch and saves it in buffers
        /// on the device.
        /// </summary>
        /// <param name="graphicsDevice">Graphicsdevice on which to save vertex and index buffers</param>
        public void Save(GraphicsDevice graphicsDevice)
        {
            Vertex[] vertices = this.PrimitiveBatch.GetVertexBatch();
            short[] indices = this.PrimitiveBatch.GetIndexBatch();
            
            // create vertex buffer on device.
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

            // Every 3 indices form 1 triangle (primitive).
            this.PrimitiveCount = indices.Length / 3;
            this.saved = true;
            
            // garbage collect primitive batch
            this.PrimitiveBatch = null;
        }

        /// <summary>
        /// Tells the graphicsdevice to prepare the buffers
        /// associated to this model for drawing.
        /// </summary>
        /// <param name="device"></param>
        public void SetToDevice(GraphicsDevice device)
        {
            if (this.saved == false)
                throw new Exception("Model was not saved");

            device.SetVertexBuffer(this.vertexBuffer);
            device.Indices = this.indexBuffer;
        }



    }
}
