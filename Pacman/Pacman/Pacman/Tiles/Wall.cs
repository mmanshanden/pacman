using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Wall : GameTile
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            return true;
        }

        public static void Load(ModelLibrary modelLibrary)
        {
            ModelBuilder mb = modelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/straight", 8);
            modelLibrary.EndModel("wall_straight");

            mb = modelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/inversecorner", 8);
            modelLibrary.EndModel("wall_corner_inverse");

            mb = modelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/corner", 8);
            modelLibrary.EndModel("wall_corner");


        }

        private int BoolToInt(bool val)
        {
            if (val)
                return 1;

            return 0;
        }

        public override void Draw(DrawManager drawManager)
        {
            bool top = this.Top is Wall;
            bool bottom = this.Bottom is Wall;
            bool left = this.Left is Wall;
            bool right = this.Right is Wall;

            int count = BoolToInt(top) + BoolToInt(left) + BoolToInt(bottom) + BoolToInt(right);

            switch (count)
            {
                case 2:
                    if (left && top)
                    {
                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner");
                        drawManager.Translate(-this.Position);
                    }
                    if (top && right)
                    {
                        drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (right && bottom)
                    {
                        drawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                    }
                    if (bottom && left)
                    {
                        drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                    }

                    break;
                case 3:
                    if (top && bottom)
                    {
                        if (right)
                        {
                            drawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                            drawManager.Translate(this.Position);
                            drawManager.DrawModel("wall_straight");
                            drawManager.Translate(-this.Position);

                            drawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                        }
                        else
                        {
                            drawManager.Translate(this.Position);
                            drawManager.DrawModel("wall_straight");
                            drawManager.Translate(-this.Position);
                        }
                    }

                    if (left && right)
                    {
                        if (top)
                        {
                            drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                            drawManager.Translate(this.Position);
                            drawManager.DrawModel("wall_straight");
                            drawManager.Translate(-this.Position);

                            drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                        }
                        else
                        {
                            drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                            drawManager.Translate(this.Position);
                            drawManager.DrawModel("wall_straight");
                            drawManager.Translate(-this.Position);

                            drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                        }
                    }
                    
                    break;

                case 4:
                    if (Right.Top is Ground)
                    {
                        drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner_inverse");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (Right.Bottom is Ground)
                    {
                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner_inverse");
                        drawManager.Translate(-this.Position);
                    }
                    if (Left.Bottom is Ground)
                    {
                        drawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner_inverse");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (Left.Top is Ground)
                    {
                        drawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                        drawManager.Translate(this.Position);
                        drawManager.DrawModel("wall_corner_inverse");
                        drawManager.Translate(-this.Position);

                        drawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                    }

                    break;
            }

     

        }
    }
}
