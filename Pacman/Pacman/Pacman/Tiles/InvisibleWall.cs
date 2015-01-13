using Base;

namespace Pacman
{
    class InvisibleWall : Ground
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            return true;
        }
    }
}
