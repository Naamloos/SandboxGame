using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Engine;
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
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new Camera(_spriteBatch, Window);
            _assetManager = new AssetManager("Fonts/HopeGold", Content);
            _assetManager.Initialize();

            _gameContext.SpriteBatch = _spriteBatch;
            _gameContext.Camera = _camera;
            _gameContext.AssetManager = _assetManager;

            _sceneManager = new SceneManager(_gameContext);
            _gameContext.SceneManager = _sceneManager;
            _sceneManager.Switch<LoadingScene>();
        }

        protected override void Update(GameTime gameTime)
        {
            _sceneManager.Update(gameTime);
            _camera.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _camera.DrawOnCamera(() =>
            {
                _sceneManager.Draw(gameTime);
            });
        }
    }
}
