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
        }
    }
}
