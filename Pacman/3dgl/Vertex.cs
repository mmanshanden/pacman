using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    /// <summary>
    /// Custom vertex decleration.
    /// </summary>
    public struct Vertex : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Color Color;

        // as required by IVertexType
        public VertexDeclaration VertexDeclaration
        {
            get
            {
                return new VertexDeclaration(
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Color, VertexElementUsage.Color, 0)
                );
            }
        }
    }
}
