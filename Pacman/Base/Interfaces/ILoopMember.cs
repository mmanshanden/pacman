using _3dgl;
using Microsoft.Xna.Framework;

namespace Base
{
    public interface ILoopMember
    {
        /// <summary>
        /// Updates the loop member.
        /// </summary>
        /// <param name="dt">Elapsed time in seconds.</param>
        void Update(float dt);
        void Draw(DrawManager drawManager); // 3d
        void Draw(DrawHelper drawHelper); // 2d
    }
}
