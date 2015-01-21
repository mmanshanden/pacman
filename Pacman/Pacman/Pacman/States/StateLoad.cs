using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateLoad : IGameState
    {
        ModelLibrary modelLibrary;

        public StateLoad(ModelLibrary modelLibrary)
        {
            this.modelLibrary = modelLibrary;
        }

        public void HandleInput(InputHelper inputHelper)
        {

        }

        public IGameState TransitionTo()
        {
            Pacman.Load(this.modelLibrary);
            Blinky.Load(this.modelLibrary);
            Clyde.Load(this.modelLibrary);
            Inky.Load(this.modelLibrary);
            Pinky.Load(this.modelLibrary);
            Ghost.Load(this.modelLibrary);

            Wall.Load(this.modelLibrary);
            GhostHouseWall.Load(this.modelLibrary);
            Boundary.Load(this.modelLibrary);
            Bubble.Load(this.modelLibrary);
            Powerup.Load(this.modelLibrary);

            return new MenuGameMode();
        }

        public void Update(float dt)
        {

        }

        public void Draw(DrawManager drawManager)
        {

        }
        public void Draw(DrawHelper drawHelper)
        {

        }

    }
}
