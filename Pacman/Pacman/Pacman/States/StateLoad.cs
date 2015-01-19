using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    class StateLoad : IGameState
    {
        public StateLoad()
        {

        }

        public void HandleInput(InputHelper inputHelper)
        {

        }

        public IGameState TransitionTo()
        {
            ObjectLoader loader = new ObjectLoader();

            Level l = new Level();
            FileReader f = new FileReader("Content/levels/level1.txt");
            l.LoadGameBoard(f.ReadGrid("level"));


            loader.Add(new Pacman());
            loader.Add(new Blinky());
            loader.Add(new Clyde());
            loader.Add(new Inky());
            loader.Add(new Pinky());
            loader.Add(new Wall());
            loader.Add(new Ground());
            loader.Add(new Ghost());
            loader.Add(new Powerup());
            loader.Add(new Bubble());

            loader.LoadObjects();

            return new StateDemo();
        }

        public void Update(float dt)
        {

        }

        public void Draw(DrawHelper drawHelper)
        {

        }

    }
}
