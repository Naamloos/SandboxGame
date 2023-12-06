using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Scenes
{
    internal class LoadingScene : BaseScene
    {
        private const string WELCOME_TEXT = "A game by Naamloos";
        private const string LOADING_TEXT = "Loading...";

        private SpriteFont _font;
        private Vector2 _textOrigin;
        private float _textScale;
        private Vector2 _screenCenter;

        private GameContext _gameContext;

        private Vector2 _loadingOrigin;

        public override void Initialize(GameContext gameContext)
        {
            _font = gameContext.AssetManager.GetFont(""); // Loads default fallback font
            _textOrigin = _font.MeasureString(WELCOME_TEXT) / 2;
            _loadingOrigin = _font.MeasureString(LOADING_TEXT) / 2;
            _textScale = 0.5f;
            _screenCenter = new Vector2(gameContext.GameWindow.ClientBounds.Width / 2, gameContext.GameWindow.ClientBounds.Height / 2);
            _gameContext = gameContext;
            _gameContext.Camera.MoveTowards(Vector2.Zero);
            _gameContext.Camera.SetSpeed(100f);
        }

        DateTimeOffset? _animationDone = null;
        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _screenCenter = new Vector2(_gameContext.GameWindow.ClientBounds.Width / 2, _gameContext.GameWindow.ClientBounds.Height / 2);

            if (_textScale < 1f)
                _textScale += (float)((0.25f / 1000f) * deltaTime);
            else if(_animationDone == null)
            {
                _animationDone = DateTimeOffset.Now;
            }
            else if(DateTimeOffset.Now.Subtract(_animationDone.Value).TotalSeconds > 3)
            {
                _gameContext.SceneManager.Switch<MenuScene>();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _gameContext.SpriteBatch.DrawString(_font, WELCOME_TEXT, _screenCenter, Color.Gold, 0, _textOrigin, _textScale, SpriteEffects.None, 1);
            if (_textScale >= 1f)
                _gameContext.SpriteBatch.DrawString(_font, LOADING_TEXT, new Vector2(_screenCenter.X, _screenCenter.Y + 30), Color.White, 0, _loadingOrigin, 0.65f, SpriteEffects.None, 1);
        }

        public override void Dispose()
        {
            // dispose if needed..
        }
    }
}
