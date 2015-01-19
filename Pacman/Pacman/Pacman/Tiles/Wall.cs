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

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/straight", 8);
            Game.DrawManager.ModelLibrary.EndModel("wall_straight");

            mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/inversecorner", 8);
            Game.DrawManager.ModelLibrary.EndModel("wall_corner_inverse");

            mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Translate(new Vector3(0, 1, 0));
            mb.PrimitiveBatch.RotateX(MathHelper.PiOver2);

            mb.BuildFromTexture("voxels/walls/corner", 8);
            Game.DrawManager.ModelLibrary.EndModel("wall_corner");


        }

        private int BoolToInt(bool val)
        {
            if (val)
                return 1;

            return 0;
        }

        public override void Draw(DrawHelper drawHelper)
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
                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner");
                        Game.DrawManager.Translate(-this.Position);
                    }
                    if (top && right)
                    {
                        Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (right && bottom)
                    {
                        Game.DrawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                    }
                    if (bottom && left)
                    {
                        Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                    }

                    break;
                case 3:
                    if (top && bottom)
                    {
                        if (right)
                        {
                            Game.DrawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                            Game.DrawManager.Translate(this.Position);
                            Game.DrawManager.DrawModel("wall_straight");
                            Game.DrawManager.Translate(-this.Position);

                            Game.DrawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                        }
                        else
                        {
                            Game.DrawManager.Translate(this.Position);
                            Game.DrawManager.DrawModel("wall_straight");
                            Game.DrawManager.Translate(-this.Position);
                        }
                    }

                    if (left && right)
                    {
                        if (top)
                        {
                            Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                            Game.DrawManager.Translate(this.Position);
                            Game.DrawManager.DrawModel("wall_straight");
                            Game.DrawManager.Translate(-this.Position);

                            Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                        }
                        else
                        {
                            Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                            Game.DrawManager.Translate(this.Position);
                            Game.DrawManager.DrawModel("wall_straight");
                            Game.DrawManager.Translate(-this.Position);

                            Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                        }
                    }
                    
                    break;

                case 4:
                    if (Right.Top is Ground)
                    {
                        Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner_inverse");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (Right.Bottom is Ground)
                    {
                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner_inverse");
                        Game.DrawManager.Translate(-this.Position);
                    }
                    if (Left.Bottom is Ground)
                    {
                        Game.DrawManager.RotateOver(-MathHelper.PiOver2, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner_inverse");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(MathHelper.PiOver2, Vector2.One * 0.5f);
                    }
                    if (Left.Top is Ground)
                    {
                        Game.DrawManager.RotateOver(-MathHelper.Pi, Vector2.One * 0.5f);

                        Game.DrawManager.Translate(this.Position);
                        Game.DrawManager.DrawModel("wall_corner_inverse");
                        Game.DrawManager.Translate(-this.Position);

                        Game.DrawManager.RotateOver(MathHelper.Pi, Vector2.One * 0.5f);
                    }

                    break;
            }

     

        }
    }
}
