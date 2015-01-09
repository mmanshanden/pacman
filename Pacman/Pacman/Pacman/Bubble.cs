using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Bubble : GameObject
    {
        public Bubble()
        {

        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.Translate(1 / 3f, 1 / 3f);
            drawHelper.Scale(1 / 3f, 1 / 3f);
            drawHelper.DrawBox(Color.White);
            drawHelper.Scale(3, 3);
            drawHelper.Translate(-1 / 3f, -1 / 3f);
            drawHelper.Translate(-this.Position);
        }
    }
}
