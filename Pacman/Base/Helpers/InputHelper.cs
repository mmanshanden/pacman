using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Base
{
    public class InputHelper
    {
        KeyboardState ksPrevious;
        KeyboardState ksCurrent;

        GamePadState gsPrevious;
        GamePadState gsCurrent;

        public InputHelper()
        {
            ksPrevious = ksCurrent = Keyboard.GetState();
        }

        private bool TranslateController(Keys key, GamePadState state)
        {
            if (!this.gsCurrent.IsConnected)
                return false;

            switch(key)
            {
                case Keys.Y:
                case Keys.R:
                    return state.IsButtonDown(Buttons.Y);
                case Keys.X:
                    return state.IsButtonDown(Buttons.X);
                case Keys.Enter:
                case Keys.A:
                    return state.IsButtonDown(Buttons.A);
                case Keys.Back:
                case Keys.B:
                    return state.IsButtonDown(Buttons.B);
                case Keys.Left:
                    return state.IsButtonDown(Buttons.DPadLeft);
                case Keys.Right:
                    return state.IsButtonDown(Buttons.DPadRight);
                case Keys.Down:
                    return state.IsButtonDown(Buttons.DPadDown);
                case Keys.Up:
                    return state.IsButtonDown(Buttons.DPadUp);
            }

            return false;
        }

        public bool KeyDown(Keys key)
        {
            return (ksCurrent.IsKeyDown(key) || TranslateController(key, gsCurrent));
        }
        public bool KeyPressed(Keys key)
        {
            return (
                (ksPrevious.IsKeyUp(key) && ksCurrent.IsKeyDown(key)) ||
                (TranslateController(key, gsPrevious) && TranslateController(key, gsCurrent))
            );
        }

        public Vector2 GetDirectionalInput()
        {
            if (KeyDown(Keys.W))
                return Collision.Up;
            if (KeyDown(Keys.S))
                return Collision.Down;
            if (KeyDown(Keys.A))
                return Collision.Left;
            if (KeyDown(Keys.D))
                return Collision.Right;

            return this.LeftStickVector();
        }

        public Vector2 LeftStickVector()
        {
            if (!this.gsCurrent.IsConnected)
                return Vector2.Zero;

            Vector2 stick = new Vector2();
            stick.X = gsCurrent.ThumbSticks.Left.X;
            stick.Y = gsCurrent.ThumbSticks.Left.Y * -1;

            return stick;
        }
        public Vector2 RightStickVector()
        {
            if (!this.gsCurrent.IsConnected)
                return Vector2.Zero;

            Vector2 stick = new Vector2();
            stick.X = gsCurrent.ThumbSticks.Right.X;
            stick.Y = gsCurrent.ThumbSticks.Right.Y * -1;

            return stick;
        }

        public void Update()
        {
            KeyboardState ks = Keyboard.GetState();
            ksPrevious = ksCurrent;
            ksCurrent = ks;

            GamePadState gs = GamePad.GetState(PlayerIndex.One);
            gsPrevious = gsCurrent;
            gsCurrent = gs;
        }
    }
}
