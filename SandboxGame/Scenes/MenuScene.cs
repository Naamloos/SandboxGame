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
    internal class MenuScene : BaseScene
    {
        private GameContext _gameContext;
        private Sprite _grassTest;
        private SpriteFont _font;

        public override void Initialize(GameContext gameContext)
        {
            _gameContext = gameContext;
            _grassTest = gameContext.AssetManager.GetSprite("tile");
            _font = gameContext.AssetManager.GetFont("main");
        }

        public override void Draw(GameTime gameTime)
        {
            _gameContext.SpriteBatch.GraphicsDevice.Clear(Color.SlateBlue);
            _grassTest.Draw(_gameContext.SpriteBatch, (int)_gameContext.Camera.ScreenCenter.X - 64, (int)_gameContext.Camera.ScreenCenter.Y - 64);
            _gameContext.SpriteBatch.DrawString(_font, "Drawn on Game layer", _gameContext.Camera.ScreenCenter, Color.Yellow);
        }

        public override void Update(GameTime gameTime)
        {
            _grassTest.Update(gameTime);

            if(!_gameContext.Camera.IsMoving())
            {
                _gameContext.Camera.SetTarget(_gameContext.Camera.ScreenCenter + new Vector2(Random.Shared.Next(-100, 100), 
                    Random.Shared.Next(-100, 100)));
                _gameContext.Camera.SetSpeed(200f);
            }
        }

        public override void DrawUI(GameTime gameTime)
        {
            _gameContext.SpriteBatch.DrawString(_font, "Drawn on UI layer (Camera Moving)", new Vector2(15, _gameContext.Camera.ScreenCenter.Y), Color.Red);
        }

        public override void Dispose()
        {

        }
    }
}
