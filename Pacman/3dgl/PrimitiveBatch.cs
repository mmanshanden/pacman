using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class PrimitiveBatch
    {
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

        public void SetColor(Color color)
        {
            this.color = color;
        }

        public void DrawQuad(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            Vector3 normal = Vector3.Cross(p2 - p1, p3 - p1);

            Vertex v1, v2, v3, v4;
            v1.Color = v2.Color = v3.Color = v4.Color = this.color;
            v1.Normal = v2.Normal = v3.Normal = v4.Normal = normal;

            v1.Position = Vector3.Transform(p1, this.transformations);
            v2.Position = Vector3.Transform(p2, this.transformations);
            v3.Position = Vector3.Transform(p3, this.transformations);
            v4.Position = Vector3.Transform(p4, this.transformations);
            
            int vertexCount = this.vertices.Count;

            this.vertices.Add(v1);
            this.vertices.Add(v2);
            this.vertices.Add(v3);
            this.vertices.Add(v4);            

            this.indices.Add((short)(vertexCount + 0));
            this.indices.Add((short)(vertexCount + 1));
            this.indices.Add((short)(vertexCount + 2));
            this.indices.Add((short)(vertexCount + 2));
            this.indices.Add((short)(vertexCount + 3));
            this.indices.Add((short)(vertexCount + 0));
        }

        public void DrawCube()
        {
            Vector3 tlf = new Vector3(0, 0, 0);
            Vector3 blf = new Vector3(0, 1, 0);
            Vector3 brf = new Vector3(1, 1, 0);
            Vector3 trf = new Vector3(1, 0, 0);
            Vector3 tlb = new Vector3(0, 0, 1);
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

        public Vertex[] GetVertexBatch()
        {
            return this.vertices.ToArray();
        }

        public short[] GetIndexBatch()
        {
            return this.indices.ToArray();
        }
    }
}
