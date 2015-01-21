using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Boundary : Wall
    {
        public Boundary()
        {

        }

        public Boundary(Orientations orientation)
        {
            this.Orientation = orientation;
        }

        public enum Orientations
        {
            Horizontal,
            Vertical,
            Corner
        }

        public Orientations Orientation { get; set; }

        public new static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();
            mb.PrimitiveBatch.Scale(new Vector3(0.5f, 1, 1));
            mb.BuildFromTexture("voxels/walls/boundary", 8);
            modelLibrary.EndModel("boundary");

            mb = modelLibrary.BeginModel();
            mb.BuildFromTexture("voxels/walls/boundarycorner", 8);
            modelLibrary.EndModel("boundary_corner");
        }

        public override void Draw(DrawManager drawManager)
        {
            switch (this.Orientation)
            {
                case Orientations.Horizontal:
                    drawManager.Rotate(MathHelper.PiOver2);

                    if (Bottom == null)
                    {
                        drawManager.Translate(this.Position + new Vector2(0, 1));
                        drawManager.DrawModel("boundary");
                        drawManager.Translate(-this.Position - new Vector2(0, 1));
                    }
                    else
                    {
                        drawManager.Translate(this.Position + new Vector2(0, 0.5f));
                        drawManager.DrawModel("boundary");
                        drawManager.Translate(-this.Position - new Vector2(0, 0.5f));
                    }

                    drawManager.Rotate(-MathHelper.PiOver2);

                    break;

                case Orientations.Vertical:
                    if (Left == null)
                    {
                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("boundary");
                        drawManager.Translate(-this.Position);
                    }
                    else
                    {
                        drawManager.Translate(this.Position + new Vector2(0.5f, 0));
                        drawManager.DrawModel("boundary");
                        drawManager.Translate(-this.Position - new Vector2(0.5f, 0));
                    }

                    break;

                case Orientations.Corner:
                    if (Bottom is Boundary)
                    {
                        if (Left is Boundary)
                        {
                            if (Top is Ground && Right is Ground)
                            {

                            }

                            else
                            {
                                drawManager.Rotate(MathHelper.PiOver2);
                                drawManager.Translate(0, 1);
                                drawManager.Translate(this.Position);
                                drawManager.DrawModel("boundary_corner");
                                drawManager.Translate(-this.Position);
                                drawManager.Translate(0, -1);
                                drawManager.Rotate(-MathHelper.PiOver2);
                            }
                        }

                        else
                        {
                            if (Top is Ground && Left is Ground)
                            {
                                
                            }
                            else
                            {
                                drawManager.Rotate(MathHelper.Pi);
                                drawManager.Translate(1, 1);
                                drawManager.Translate(this.Position);
                                drawManager.DrawModel("boundary_corner");
                                drawManager.Translate(-this.Position);
                                drawManager.Translate(-1, -1);
                                drawManager.Rotate(-MathHelper.Pi);
                            }
                        }
                    }

                    else
                    {
                        if (Left is Boundary)
                        {
                            if (Bottom is Ground && Right is Ground)
                            {

                            }
                            else 
                            {                                
                                drawManager.Translate(this.Position);
                                drawManager.DrawModel("boundary_corner");
                                drawManager.Translate(-this.Position);
                            }
                        }
                        else
                        {
                            if (Bottom is Ground && Left is Ground)
                            {

                            }
                            else
                            {
                                drawManager.Rotate(-MathHelper.PiOver2);
                                drawManager.Translate(1, 0);
                                drawManager.Translate(this.Position);
                                drawManager.DrawModel("boundary_corner");
                                drawManager.Translate(-this.Position);
                                drawManager.Translate(-1, 0);
                                drawManager.Rotate(MathHelper.PiOver2);
                            }
                        }
                    }

                    break;

            }
        }
    }
}
