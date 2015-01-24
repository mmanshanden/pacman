using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3dgl
{
    /// <summary>
    /// Contains properties which alter the view
    /// matrix.
    /// </summary>
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
            // translate position in 2d space
            // to position in 3d space:
            // v2.x = v3.x; v2.y = v3.z;
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
            // calculate camera position using phi and rho angles.
            // Rho is angle between camera y position and x,z plane
            // Phi is angle between camera z position and x axis
            // zoom is distance between origin and camera position.
            Vector3 cameraPos = new Vector3();
            cameraPos.X = (float)Math.Cos(Rho) * Zoom * (float)Math.Sin(Phi);
            cameraPos.Y = (float)Math.Sin(Rho) * Zoom;
            cameraPos.Z = (float)Math.Cos(Rho) * Zoom * (float)Math.Cos(Phi);

            effect.View =  Matrix.CreateLookAt(cameraPos + this.target, this.target, Vector3.Up);


            if (!this.FreeAim)
            {
                // difference from rho=1
                float dy = Math.Abs(this.Rho - 1);

                // rest at 0.7
                if (this.Rho > 1)                    
                    this.Rho -= dy * 0.06f; // allow more movement up

                if (this.Rho < 1)
                    this.Rho += dy * 0.09f;

                // rest at 0
                this.Phi -= this.Phi * 0.065f;

                // keep zoom at 18
                float dz = this.Zoom - 14;
                this.Zoom -= dz * 0.065f;

            }
        }
    }
}
