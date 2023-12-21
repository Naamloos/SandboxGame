﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
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

        public override void Initialize()
        {
            _font = GameContext.AssetManager.GetFont("main");
            _textOrigin = _font.MeasureString(WELCOME_TEXT) / 2;
            _textScale = 0.5f;
            _screenCenter = new Vector2(GameContext.GameWindow.ClientBounds.Width / 2, GameContext.GameWindow.ClientBounds.Height / 2);

            _finishedTime = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(5);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            _screenCenter = new Vector2(GameContext.GameWindow.ClientBounds.Width / 2, GameContext.GameWindow.ClientBounds.Height / 2);

            if (_textScale < 1f)
                _textScale += (float)((0.25f / 1000f) * deltaTime);

            if(_finishedTime < DateTimeOffset.UtcNow)
            {
                GameContext.SceneManager.Switch<InGameScene>();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            GameContext.Camera.DrawToUI(() =>
            {
                GameContext.SpriteBatch.DrawString(_font, WELCOME_TEXT, _screenCenter, Color.Gold, 0, _textOrigin, _textScale, SpriteEffects.None, 1);
            });
        }

        public override void Dispose()
        {
            // dispose if needed..
        }
    }
}
