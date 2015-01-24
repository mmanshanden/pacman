using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    /// <summary>
    /// Batches quads together. 
    /// </summary>
    public class PrimitiveBatch
    {
        // transformations applied to quads
        Matrix transformations;

        List<Vertex> vertices;
        List<short> indices;
        Color color;


        public PrimitiveBatch()
        {
            this.transformations = Matrix.Identity;
            this.vertices = new List<Vertex>();
            this.indices = new List<short>();
            this.color = Color.White;
        }

        #region Transformations
        public void Translate(Vector3 translation)
        {
            this.transformations = Matrix.CreateTranslation(translation) * this.transformations;
        }
        public void Scale(Vector3 scale)
        {
            this.transformations = Matrix.CreateScale(scale) * this.transformations;
        }
        public void RotateX(float radians)
        {
            this.transformations = Matrix.CreateRotationX(radians) * this.transformations;
        }
        public void RotateY(float radians)
        {
            this.transformations = Matrix.CreateRotationY(radians) * this.transformations;
        }
        public void RotateZ(float radians)
        {
            this.transformations = Matrix.CreateRotationZ(radians) * this.transformations;
        }
        #endregion
        
        /// <summary>
        /// Sets the color for all new vertices added to the batch.
        /// </summary>
        /// <param name="color">Vertex color</param>
        public void SetColor(Color color)
        {
            this.color = color;
        }

        /// <summary>
        /// Draws a quad between four points.
        /// </summary>
        public void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {           
            Vertex v1, v2, v3, v4;
            v1.Color = v2.Color = v3.Color = v4.Color = this.color; 

            // Transform 4 given points with transformation matrix
            v1.Position = Vector3.Transform(p1, this.transformations);
            v2.Position = Vector3.Transform(p2, this.transformations);
            v3.Position = Vector3.Transform(p3, this.transformations);
            v4.Position = Vector3.Transform(p4, this.transformations);

            // calculate new normal using transformed points. Using transformed positions are
            // vital for proper lighting.
            Vector3 normal = Vector3.Cross(v2.Position - v1.Position, v3.Position - v1.Position);
            normal.Normalize(); // convert to normal vector (vector of length 1).
            v1.Normal = v2.Normal = v3.Normal = v4.Normal = normal;

            int vertexCount = this.vertices.Count; // get current vertex count to base indices on

            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);
            this.vertices.Add(v4);            

            // A quad contains 2 triangles. Each triangle contains 3 vertices. 
            // The indices tell the graphicsdevice which vertices to use for 
            // drawing a triangle.
            this.indices.Add((short)(vertexCount + 0));
            this.indices.Add((short)(vertexCount + 1));
            this.indices.Add((short)(vertexCount + 2));
            this.indices.Add((short)(vertexCount + 2));
            this.indices.Add((short)(vertexCount + 3));
            this.indices.Add((short)(vertexCount + 0));
        }

        /// <summary>
        /// Draws 6 quads to form a cube. The original dimensions of
        /// the cube will be 1x1x1.
        /// </summary>
        public void DrawCube()
        {
            Vector3 tlf = new Vector3(0, 0, 0); // top left front corner
            Vector3 blf = new Vector3(0, 1, 0); // bottom left front
            Vector3 brf = new Vector3(1, 1, 0); // bottom right ..
            Vector3 trf = new Vector3(1, 0, 0); // top right..
            Vector3 tlb = new Vector3(0, 0, 1); // etc..
            Vector3 blb = new Vector3(0, 1, 1);
            Vector3 brb = new Vector3(1, 1, 1);
            Vector3 trb = new Vector3(1, 0, 1);

            this.DrawQuad(tlf, trf, brf, blf); // front
            this.DrawQuad(trf, trb, brb, brf); // right
            this.DrawQuad(trb, tlb, blb, brb); // back
            this.DrawQuad(tlb, tlf, blf, blb); // left
            this.DrawQuad(blf, brf, brb, blb); // bottom
            this.DrawQuad(tlf, trf, trb, tlb); // top
        }

        /// <summary>
        /// Returns the array of vertices. Useful for setting
        /// vertex buffer data.
        /// </summary>
        /// <returns>Array of vertices.</returns>
        public Vertex[] GetVertexBatch()
        {
            return this.vertices.ToArray();
        }

        /// <summary>
        /// Returns the array of indices. Every 6 indices form 1
        /// quad. Useful for setting index buffer data.
        /// </summary>
        /// <returns>Array of indices</returns>
        public short[] GetIndexBatch()
        {
            return this.indices.ToArray();
        }
    }
}
