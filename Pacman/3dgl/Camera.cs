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
        public bool FreeAim
        {
            get;
            set;
        }

        private BasicEffect effect;
        private Vector3 target;

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

        public Vector2 Target
        {
            get
            {
                Vector2 target = new Vector2();
                target.X = this.target.X;
                target.Y = this.target.Z;

                return target;
            }
            set
            {
                this.target.X = value.X;
                this.target.Z = value.Y;
            }
        }
        public void SetCameraHeight(float height)
        {
            this.target.Y = height;
        }

        public Camera(BasicEffect effect)
        {
            this.effect = effect;
            this.FreeAim = false;
        }

        public void Update()
        {
  
            Vector3 cameraPos = new Vector3();
            cameraPos.X = (float)Math.Cos(Rho) * Zoom * (float)Math.Sin(Phi);
            cameraPos.Y = (float)Math.Sin(Rho) * Zoom;
            cameraPos.Z = (float)Math.Cos(Rho) * Zoom * (float)Math.Cos(Phi);

            effect.View =  Matrix.CreateLookAt(cameraPos + this.target, this.target, Vector3.Up);


            if (!this.FreeAim)
            {
                float dy = Math.Abs(this.Rho - 1);

                // rest at 0.7, allow more movement up
                if (this.Rho > 1)                    
                    this.Rho -= dy * 0.06f;

                if (this.Rho < 1)
                    this.Rho += dy * 0.18f;

                // rest at 0
                this.Phi -= this.Phi * 0.065f;

                // keep zoom at 18
                float dz = this.Zoom - 14;
                this.Zoom -= dz * 0.065f;

            }
        }
    }
}
