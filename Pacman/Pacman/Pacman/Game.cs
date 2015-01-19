using _3dgl;
using Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pacman
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        private DrawHelper drawHelper;
        private InputHelper inputHelper;

        public static DrawManager DrawManager
        {
            get;
            set;
        }

        protected IGameState gameState;

        public static Vector2 Screen
        {
            get;
            private set;
        }
        public static Random Random
        {
            get;
            private set;
        }

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.gameState = new StateDefault();

            this.graphics.PreferredBackBufferWidth = 1920;
            this.graphics.PreferredBackBufferHeight = 1080;

            Game.Random = new Random();
        }

        protected override void LoadContent()
        {
            this.drawHelper = new DrawHelper(GraphicsDevice);
            this.inputHelper = new InputHelper();

            this.drawHelper.LoadTextures(Content);

            Game.DrawManager = new DrawManager(GraphicsDevice, Content);

            Game.DrawManager.Initialize();
            Console.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            Game.DrawManager.Camera.Update();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.inputHelper.Update();

            Vector2 rs = inputHelper.RightStickVector();
            Game.DrawManager.Camera.Phi += rs.X * 0.1f;
            Game.DrawManager.Camera.Rho += rs.Y * -0.1f;

            if (this.inputHelper.KeyPressed(Keys.OemTilde))
                Console.Visible = !Console.Visible;

            this.gameState = this.gameState.TransitionTo();

            this.gameState.HandleInput(this.inputHelper);
            this.gameState.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            Game.DrawManager.BeginDraw();

            this.drawHelper.SpriteBatch.Begin();
            this.gameState.Draw(drawHelper);
            this.drawHelper.SpriteBatch.End();

            Console.Draw();

            base.Draw(gameTime);
        }
    }
}
