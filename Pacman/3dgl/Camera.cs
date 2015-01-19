using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    public class Camera
    {
        private BasicEffect effect;

        public float Rho
        {
            get;
            set;
        }
        public float Phi
        {
            get;
            set;
        }

        public float Zoom
        {
            get;
            set;
        }

        public Vector3 Target
        {
            get;
            set;
        }

        public Camera(BasicEffect effect)
        {
            this.effect = effect;
            this.Zoom = 5;
        }

        public void Update()
        {
            Vector3 cameraPos = new Vector3();
            cameraPos.X = (float)Math.Cos(Rho) * Zoom * (float)Math.Sin(Phi);
            cameraPos.Y = (float)Math.Sin(Rho) * Zoom;
            cameraPos.Z = (float)Math.Cos(Rho) * Zoom * (float)Math.Cos(Phi);

            effect.View =  Matrix.CreateLookAt(cameraPos + Target, Target, Vector3.Up);
        }
    }
}
