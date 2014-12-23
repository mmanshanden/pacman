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
        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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

            base.Draw(gameTime);
        }
    }
}
