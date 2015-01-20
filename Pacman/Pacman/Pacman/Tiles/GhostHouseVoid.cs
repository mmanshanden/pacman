using Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pacman
{
    class GhostHouseVoid : Ground
    {
        public override bool IsCollidable(GameObject gameObject)
        {
            return true;
        }


    }
}
