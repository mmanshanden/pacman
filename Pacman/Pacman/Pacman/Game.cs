using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pacman
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private DrawHelper drawHelper;
        private InputHelper inputHelper;

        protected IGameState gameState;

        public static Vector2 Screen
        {
            get;
            private set;
        }

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.gameState = new StateDefault();
        }

        protected override void LoadContent()
        {
            this.drawHelper = new DrawHelper(GraphicsDevice);
            this.inputHelper = new InputHelper();

            this.drawHelper.LoadTextures(Content);

            Console.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.inputHelper.Update();

            if (this.inputHelper.KeyPressed(Keys.OemTilde))
                Console.Visible = !Console.Visible;

            this.gameState = this.gameState.TransitionTo();

            this.gameState.HandleInput(this.inputHelper);
            this.gameState.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.White);

            this.drawHelper.SpriteBatch.Begin();
            this.gameState.Draw(drawHelper);
            this.drawHelper.SpriteBatch.End();

            Console.Draw();

            base.Draw(gameTime);
        }
    }
}
