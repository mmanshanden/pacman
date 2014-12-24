using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Base
{
    public class InputHelper
    {
        KeyboardState ksPrevious;
        KeyboardState ksCurrent;

        public InputHelper()
        {
            ksPrevious = ksCurrent = Keyboard.GetState();
        }

        public bool KeyDown(Keys key)
        {
            return ksCurrent.IsKeyDown(key);
        }
        public bool KeyPressed(Keys key)
        {
            return (ksPrevious.IsKeyUp(key) && ksCurrent.IsKeyDown(key));
        }

        public Vector2 GetDirectionalInput()
        {
            Vector2 direction = new Vector2();
            if (KeyDown(Keys.W))
                direction.Y -= 1;
            if (KeyDown(Keys.S))
                direction.Y += 1;
            if (KeyDown(Keys.A))
                direction.X -= 1;
            if (KeyDown(Keys.D))
                direction.X += 1;

            return direction;
        }

        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            ksPrevious = ksCurrent;
            ksCurrent = ks;
        }
    }
}
