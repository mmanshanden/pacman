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

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Blue);
            drawHelper.Translate(-this.Position);

            Game.DrawManager.Translate(this.Position.X, this.Position.Y);
            Game.DrawManager.DrawModel("block");
            Game.DrawManager.Translate(-this.Position.X, -this.Position.Y);
        }
    }
}
