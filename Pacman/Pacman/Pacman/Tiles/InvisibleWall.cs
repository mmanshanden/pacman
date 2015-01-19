using Base;

namespace Pacman
{
    class InvisibleWall : Ground
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            if (gameObject is Ghost)
            {
                Ghost ghost = (Ghost)gameObject;
                if (ghost.State == Ghost.States.Leave || ghost.State == Ghost.States.Dead || ghost.State == Ghost.States.Wait)
                return false;
            }

            return true;
        }
    }
}
