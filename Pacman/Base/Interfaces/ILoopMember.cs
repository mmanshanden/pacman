﻿using Microsoft.Xna.Framework;

namespace Base
{
    public interface ILoopMember
    {
        void Update(float dt);
        void Draw(DrawHelper drawHelper);
    }
}
