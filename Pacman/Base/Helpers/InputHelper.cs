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
            if (KeyDown(Keys.W))
                return Vector2.UnitY * -1;
            if (KeyDown(Keys.S))
                return Vector2.UnitY;
            if (KeyDown(Keys.A))
                return Vector2.UnitX * -1;
            if (KeyDown(Keys.D))
                return Vector2.UnitX;

            return Vector2.Zero;
        }

        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            ksPrevious = ksCurrent;
            ksCurrent = ks;
        }
    }
}
