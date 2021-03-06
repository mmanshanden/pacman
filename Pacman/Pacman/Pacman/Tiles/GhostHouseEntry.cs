﻿using Base;

namespace Pacman
{
    class GhostHouseEntry : Ground
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            Ghost ghost = gameObject as Ghost;

            if (ghost == null)
                return true;

            if (ghost.State == Ghost.States.Dead || 
                ghost.State == Ghost.States.Leave ||
                ghost.State == Ghost.States.Wait)
                return false;

            return true;
        }
    }
}
