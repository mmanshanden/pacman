using Base;

namespace Pacman
{
    class GhostHouseEntry : Ground
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            Ghost ghost = gameObject as Ghost;

            if (ghost == null)
                return true;

            if (ghost.State == Ghost.States.Dead)
                return false;

            return true;
        }
    }
}
