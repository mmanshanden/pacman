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

        public void SetTarget(Vector2 target)
        {
            this.Target = new Vector3(target.X, this.Target.Y, target.Y);
        }
        public void SetCameraHeight(float height)
        {
            this.Target = new Vector3(this.Target.X, height, this.Target.Z);
        }

        public Camera(BasicEffect effect)
        {
            this.effect = effect;
            this.Zoom = 18;
            this.Rho = 1;
        }

        public void Update()
        {
  
            Vector3 cameraPos = new Vector3();
            cameraPos.X = (float)Math.Cos(Rho) * Zoom * (float)Math.Sin(Phi);
            cameraPos.Y = (float)Math.Sin(Rho) * Zoom;
            cameraPos.Z = (float)Math.Cos(Rho) * Zoom * (float)Math.Cos(Phi);

            effect.View =  Matrix.CreateLookAt(cameraPos + Target, Target, Vector3.Up);

            Console.WriteLine("Phi: " + this.Phi);
            Console.WriteLine("Rho: " + this.Rho);
        }
    }
}
