using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Scenes;
using System;

namespace SandboxGame.Scenes
{
    internal class WelcomeScene : BaseScene
    {
        private const string WELCOME_TEXT = "A game by Naamloos";

        private SpriteFont _font;
        private Vector2 _textOrigin;
        private float _textScale;
        private Vector2 _screenCenter;

        private DateTimeOffset _finishedTime;

        private AssetManager _assetManager;
        private GameWindow _gameWindow;
        private SceneManager _sceneManager;
        private Camera _camera;
        private SpriteBatch _spriteBatch;

        public WelcomeScene(AssetManager assetManager, GameWindow gameWindow, SceneManager sceneManager, Camera camera, SpriteBatch spriteBatch)
        {
            this._sceneManager = sceneManager;
            this._assetManager = assetManager;
            this._gameWindow = gameWindow;
            this._camera = camera;
            this._spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {
            _font = _assetManager.GetFont("main");
            _textOrigin = _font.MeasureString(WELCOME_TEXT) / 2;
            _textScale = 0.5f;
            _screenCenter = new Vector2(_gameWindow.ClientBounds.Width / 2, _gameWindow.ClientBounds.Height / 2);

            _finishedTime = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(5);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _screenCenter = new Vector2(_gameWindow.ClientBounds.Width / 2, _gameWindow.ClientBounds.Height / 2);

            if (_textScale < 1f)
                _textScale += (float)((0.25f / 1000f) * deltaTime);

            if(_finishedTime < DateTimeOffset.UtcNow)
            {
                _sceneManager.Switch<InGameScene>();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _camera.DrawToUI(() =>
            {
                _spriteBatch.DrawString(_font, WELCOME_TEXT, _screenCenter, Color.Gold, 0, _textOrigin, _textScale, SpriteEffects.None, 1);
            });
        }

        public override void Dispose()
        {
            // dispose if needed..
        }
    }
}
