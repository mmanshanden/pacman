using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Boundary : Wall
    {
        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();
            mb.PrimitiveBatch.Scale(new Vector3(0.5f, 1, 1));
            mb.BuildFromTexture("voxels/walls/boundary", 8);
            Game.DrawManager.ModelLibrary.EndModel("boundary");



        }

        public override void Draw(Base.DrawHelper drawHelper)
        {
            bool top = Top is Ground;
            bool right = Right is Ground;
            bool bottom = Bottom is Ground;

            

            Game.DrawManager.Translate(this.Position);
            //Game.DrawManager.DrawModel("boundary");
            Game.DrawManager.Translate(-this.Position);


        }
    }
}
