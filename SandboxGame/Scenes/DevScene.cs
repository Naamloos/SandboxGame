using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Scenes
{
    public class DevScene : BaseScene
    {
        private GameContext _gameContext;
        private Player _player;
        private Sprite _grass;
        private Sprite _dummyGuy;

        private const float MAX_ZOOM = 6f;
        private const float MIN_ZOOM = 0.5f;
        private float _zoom = 3f;

        public override void Initialize(GameContext gameContext)
        {
            _gameContext = gameContext;
            _player = new Player(gameContext.AssetManager.GetSprite("player"), gameContext.InputHelper);
            _grass = gameContext.AssetManager.GetSprite("grass");
            _dummyGuy = gameContext.AssetManager.GetSprite("player").Copy();
            _gameContext.Camera.Follow(_player.Sprite, false);
            _gameContext.Camera.SetSpeed(250);
        }

        private bool touchDummyGuy = false;

        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < (15 * 15); i++)
            {
                int x = i % 15;
                int y = (i - x) / 15;

                _grass.Draw(_gameContext.SpriteBatch, x * 32, y * 32);
            }

            _dummyGuy.Draw(_gameContext.SpriteBatch, 350, 400, touchDummyGuy, camera: _gameContext.Camera);

            _player.Draw(_gameContext.SpriteBatch, _gameContext.Camera);
        }

        public override void DrawUI(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _grass.Update(gameTime);
            _player.Update(gameTime);
            _dummyGuy.Update(gameTime);

            if(_gameContext.MouseHelper.ScrollUp && _zoom < MAX_ZOOM)
            {
                _zoom += 0.15f;
            }
            if(_gameContext.MouseHelper.ScrollDown && _zoom > MIN_ZOOM)
            {
                _zoom -= 0.15f;
            }

            // dummyguy code
            var mouseBox = new Rectangle(_gameContext.MouseHelper.WorldPos.ToPoint(), new Point(1, 1));
            if(mouseBox.Intersects(_dummyGuy.Bounds))
            {
                touchDummyGuy = true;
                if (_gameContext.MouseHelper.LeftClick)
                {
                    if (_gameContext.Camera.Target == _dummyGuy)
                    {
                        _gameContext.Camera.Follow(_player.Sprite, false);
                    }
                    else
                    {
                        _gameContext.Camera.Follow(_dummyGuy);
                    }
                }
            }
            else
            {
                touchDummyGuy = false;
            }

            _gameContext.Camera.SetZoom(_zoom);
        }

        public override void Dispose()
        {
        }
    }
}
