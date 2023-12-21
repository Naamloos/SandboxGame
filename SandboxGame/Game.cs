using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Scenes;
using SandboxGame.Scenes;
using System;

namespace SandboxGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private AssetManager _assetManager;
        private SceneManager _sceneManager;
        private Camera _camera;

        private GameContext _gameContext;
        private SpriteFont _debugTextFont;

        public Game()
        {
            _gameContext = new GameContext();
            _graphics = new GraphicsDeviceManager(this);
            _gameContext.GameWindow = Window;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = false; // set to false to unlock frame rate.
        }

        protected override void Initialize()
        {
            base.Initialize();
            Window.Title = "🥺";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera(_spriteBatch, Window);
            _assetManager = new AssetManager(Content);
            _assetManager.Initialize();
            _debugTextFont = _assetManager.GetFont();

            _gameContext.MouseHelper = new MouseHelper(Window, _camera);
            _camera.SetMouseHelper(_gameContext.MouseHelper);
            _gameContext.InputHelper = new InputHelper();
            _gameContext.SpriteBatch = _spriteBatch;
            _gameContext.Camera = _camera;
            _gameContext.AssetManager = _assetManager;

            _sceneManager = new SceneManager(_gameContext);
            _gameContext.SceneManager = _sceneManager;
            _sceneManager.Switch<DevScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            _sceneManager.Update(gameTime);
            _camera.Update(gameTime);
            _gameContext.MouseHelper.Update();
            _gameContext.InputHelper.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            DebugHelper.SetDebugValues("FRAMERATE", Math.Round((1.0f / gameTime.ElapsedGameTime.TotalSeconds)).ToString());
            GraphicsDevice.Clear(Color.Black);

            // Camera layer
            _camera.DrawOnCamera(() =>
            {
                _sceneManager.Draw(gameTime);
            });

            // UI layer
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
            var dbg = DebugHelper.GetDebugString();
            _spriteBatch.DrawString(_debugTextFont, dbg, new Vector2(10, 10), Color.DarkRed);
            _camera.FlushUIDraw();
            _spriteBatch.End();
        }
    }
}
