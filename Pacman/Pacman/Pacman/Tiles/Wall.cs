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

        public override void Load(ModelBuilder model)
        {
            Vector3 pos = new Vector3(this.Position.X, 0, this.Position.Y);

            model.PrimitiveBatch.Translate(pos);
            model.PrimitiveBatch.SetColor(Color.Blue);
            model.PrimitiveBatch.DrawCube();
            model.PrimitiveBatch.Translate(-pos);
        }
    }
}
