using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pacman
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private DrawHelper drawHelper;
        private InputHelper inputHelper;

        protected IGameState gameState;

        public static XnaConsole Console
        {
            get;
            private set;
        }
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

            Console = new XnaConsole();
        }

        protected override void LoadContent()
        {
            this.drawHelper = new DrawHelper(GraphicsDevice);
            this.inputHelper = new InputHelper();

            this.drawHelper.LoadTextures(Content);


            Console.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.inputHelper.Update();

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

            Console.Draw(
                this.drawHelper.SpriteBatch,
                new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
            );

            this.drawHelper.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
