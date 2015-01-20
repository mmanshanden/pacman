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

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Scale(new Vector3(0.5f, 1, 1));
            mb.BuildFromTexture("voxels/walls/boundary", 8);
            Game.DrawManager.ModelLibrary.EndModel("boundary");

            mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.BuildFromTexture("voxels/walls/boundarycorner", 8);
            Game.DrawManager.ModelLibrary.EndModel("boundary_corner");
        }

        public override void Draw(DrawHelper drawHelper)
        {
            switch (this.Orientation)
            {
                case Orientations.Horizontal:
                    Game.DrawManager.Rotate(MathHelper.PiOver2);

                    if (Bottom == null)
                    {
                        Game.DrawManager.Translate(this.Position + new Vector2(0, 1));
                        Game.DrawManager.DrawModel("boundary");
                        Game.DrawManager.Translate(-this.Position - new Vector2(0, 1));
                    }
                    else
                    {
                        Game.DrawManager.Translate(this.Position + new Vector2(0, 0.5f));
                        Game.DrawManager.DrawModel("boundary");
                        Game.DrawManager.Translate(-this.Position - new Vector2(0, 0.5f));
                    }

                    Game.DrawManager.Rotate(-MathHelper.PiOver2);

                    break;

                case Orientations.Vertical:
                    if (Left == null)
                    {
                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("boundary");
                        Game.DrawManager.Translate(-this.Position);
                    }
                    else
                    {
                        Game.DrawManager.Translate(this.Position + new Vector2(0.5f, 0));
                        Game.DrawManager.DrawModel("boundary");
                        Game.DrawManager.Translate(-this.Position - new Vector2(0.5f, 0));
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
                                Game.DrawManager.Rotate(MathHelper.PiOver2);
                                Game.DrawManager.Translate(0, 1);
                                Game.DrawManager.Translate(this.Position);
                                Game.DrawManager.DrawModel("boundary_corner");
                                Game.DrawManager.Translate(-this.Position);
                                Game.DrawManager.Translate(0, -1);
                                Game.DrawManager.Rotate(-MathHelper.PiOver2);
                            }
                        }

                        else
                        {
                            if (Top is Ground && Left is Ground)
                            {
                                
                            }
                            else
                            {
                                Game.DrawManager.Rotate(MathHelper.Pi);
                                Game.DrawManager.Translate(1, 1);
                                Game.DrawManager.Translate(this.Position);
                                Game.DrawManager.DrawModel("boundary_corner");
                                Game.DrawManager.Translate(-this.Position);
                                Game.DrawManager.Translate(-1, -1);
                                Game.DrawManager.Rotate(-MathHelper.Pi);
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
                                Game.DrawManager.Translate(this.Position);
                                Game.DrawManager.DrawModel("boundary_corner");
                                Game.DrawManager.Translate(-this.Position);
                            }
                        }
                        else
                        {
                            if (Bottom is Ground && Left is Ground)
                            {

                            }
                            else
                            {
                                Game.DrawManager.Rotate(-MathHelper.PiOver2);
                                Game.DrawManager.Translate(1, 0);
                                Game.DrawManager.Translate(this.Position);
                                Game.DrawManager.DrawModel("boundary_corner");
                                Game.DrawManager.Translate(-this.Position);
                                Game.DrawManager.Translate(-1, 0);
                                Game.DrawManager.Rotate(MathHelper.PiOver2);
                            }
                        }
                    }

                    break;

            }
        }
    }
}
