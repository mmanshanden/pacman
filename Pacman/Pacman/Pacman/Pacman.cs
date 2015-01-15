using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Pacman : GameCharacter
    {
        public Pacman()
        {

        }

        public override void Draw(DrawHelper drawHelper)
        {
            drawHelper.Translate(this.Position);
            drawHelper.DrawBox(Color.Yellow);
            drawHelper.Translate(-this.Position);
        }
    }
}