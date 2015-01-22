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

        private SoundHelper soundHelper;
        private InputHelper inputHelper;

        public static Camera Camera { get; private set; }
        public static Vector2 Screen { get; private set; }
        public static Random Random { get; private set; }
        public static SoundHelper SoundManager { get; private set; }

        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 1280;
            this.graphics.PreferredBackBufferHeight = 720;
            this.graphics.ApplyChanges();

            this.Window.AllowUserResizing = true;

            Game.Random = new Random();
        }

        protected override void LoadContent()
        {
            this.drawHelper2d = new DrawHelper(GraphicsDevice);
            this.soundHelper = new SoundHelper();
            this.inputHelper = new InputHelper();

            Game.SoundManager = this.soundHelper;

            // 2d loading
            this.drawHelper2d.LoadTextures(this.Content);

            // 3d initialization & loading
            this.drawHelper3d = new DrawManager(GraphicsDevice, Content);
            this.drawHelper3d.Initialize();
            Game.Camera = this.drawHelper3d.Camera;

            // load 3d models
            this.gameState = new StateLoad(this.drawHelper3d.ModelLibrary);

            // load audio files
            this.soundHelper.LoadAudio(this.Content);

            // initialize console
            Console.Initialize(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            drawHelper3d.Camera.Update();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.inputHelper.Update();

            Vector2 screen = new Vector2();
            screen.X = GraphicsDevice.Viewport.Width;
            screen.Y = GraphicsDevice.Viewport.Height;
            this.drawHelper2d.Screen = screen;

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

            // show/hide console
            if (this.inputHelper.KeyPressed(Keys.OemTilde))
                Console.Visible = !Console.Visible;

            // enable/disable sound
            if (this.inputHelper.KeyPressed(Keys.F1))
                Game.SoundManager.Enabled = !Game.SoundManager.Enabled;

            // enable/disable freeaim
            if (this.inputHelper.KeyPressed(Keys.F2))
                Game.Camera.FreeAim = !Game.Camera.FreeAim;

            this.gameState = this.gameState.TransitionTo();

            this.gameState.HandleInput(this.inputHelper);
            this.gameState.Update(dt);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.Black);

            // draw 3d
            this.drawHelper3d.BeginDraw();
            this.gameState.Draw(drawHelper3d);

            // draw 2d (ui)
            this.drawHelper2d.SpriteBatch.Begin();
            this.gameState.Draw(drawHelper2d);
            this.drawHelper2d.SpriteBatch.End();

            Console.Draw();

            base.Draw(gameTime);
        }
    }
}
