using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Ground : GameTile
    {
        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Black);
            drawHelper.Translate(-this.Position);

            Game.DrawManager.Translate(this.Position);
            //Game.DrawManager.DrawModel("floor");
            Game.DrawManager.Translate(-this.Position);
        }

        public override void Load()
        {
            ModelBuilder mb = Game.DrawManager.ModelLibrary.BeginModel();

            Vector3 p1 = new Vector3(0, 0, 0);
            Vector3 p2 = new Vector3(0, 0, 1);
            Vector3 p3 = new Vector3(1, 0, 1);
            Vector3 p4 = new Vector3(1, 0, 0);

            mb.PrimitiveBatch.DrawQuad(p1, p2, p3, p4);

            Game.DrawManager.ModelLibrary.EndModel("floor");
            
        }

    }
}
