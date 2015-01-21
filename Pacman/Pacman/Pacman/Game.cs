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
        private IGameState gameState;
                
        private DrawManager drawHelper3d;
        private DrawHelper drawHelper2d;
        private InputHelper inputHelper;

        public static Camera Camera { get; private set; }

        

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

            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;

            Game.Random = new Random();
        }

        protected override void LoadContent()
        {
            this.drawHelper2d = new DrawHelper(GraphicsDevice);
            this.inputHelper = new InputHelper();

            this.drawHelper2d.LoadTextures(this.Content);

            this.drawHelper3d = new DrawManager(GraphicsDevice, Content);
            this.drawHelper3d.Initialize();
            Game.Camera = this.drawHelper3d.Camera;

            this.gameState = new StateLoad(this.drawHelper3d.ModelLibrary);

            Console.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            drawHelper3d.Camera.Update();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.inputHelper.Update();

            // move camera controller
            Vector2 rs = inputHelper.RightStickVector();
            Camera.Phi += rs.X * 0.1f;
            Camera.Rho += rs.Y * -0.1f;

            // move camera keyboard
            if (inputHelper.KeyDown(Keys.Up))
                Camera.Rho += 0.03f;
            if (inputHelper.KeyDown(Keys.Down))
                Camera.Rho -= 0.03f;
            if (inputHelper.KeyDown(Keys.Left))
                Camera.Phi -= 0.03f;
            if (inputHelper.KeyDown(Keys.Right))
                Camera.Phi += 0.03f;

            // zoom camera
            if (inputHelper.KeyDown(Keys.PageUp))
                Camera.Zoom -= 0.5f;
            if (inputHelper.KeyDown(Keys.PageDown))
                Camera.Zoom += 0.5f;

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

            this.drawHelper3d.BeginDraw();
            this.gameState.Draw(drawHelper3d);

            Console.Draw();

            base.Draw(gameTime);
        }
    }
}
