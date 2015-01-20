using _3dgl;
using Base;
using Microsoft.Xna.Framework;

namespace Pacman
{
    class Ground : GameTile
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            return false;
        }
    }
}
