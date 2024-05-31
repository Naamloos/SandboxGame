using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Scenes;

namespace SandboxGame.Scenes
{
    internal class MenuScene : BaseScene
    {
        private LoadedSprite _grassTest;
        private SpriteFont _font;

        private AssetManager _assetManager;
        private Camera _camera;
        private GameWindow _gameWindow;
        private MouseHelper _mouseHelper;
        private SpriteBatch _spriteBatch;

        public MenuScene(AssetManager assetManager, Camera camera, GameWindow gameWindow, MouseHelper mouseHelper, SpriteBatch spriteBatch)
        {
            _assetManager = assetManager;
            _camera = camera;
            _gameWindow = gameWindow;
            _mouseHelper = mouseHelper;
            _spriteBatch = spriteBatch;
        }

        public override void Initialize()
        {
            _grassTest = _assetManager.GetSprite("tile");
            _font = _assetManager.GetFont("main");
            _camera.SetSpeed(200f);
        }

        private int posX = 500;
        private bool goesLeft = false;

        public override void Update(GameTime gameTime)
        {
            _grassTest.Update();

            if (_mouseHelper.LeftClick)
            {
                if (_camera.IsFollowing)
                {
                    _camera.StopFollowing(false);
                }
                else
                {
                    _camera.Follow(_grassTest, smooth: false);
                }
            }

            if (posX > 700)
            {
                goesLeft = true;
            }
            else if (posX < 300)
            {
                goesLeft = false;
            }

            posX += goesLeft ? -5 : 5;
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.GraphicsDevice.Clear(Color.SlateBlue);
            _grassTest.Draw(posX, 200, camera: _camera, interactable: true);
            _spriteBatch.DrawString(_font, "Drawn on Game layer", new Vector2((int)_camera.ScreenCenter.X, (int)_camera.ScreenCenter.Y), Color.Yellow);

            _camera.DrawToUI(() =>
            {
                var mouse = Mouse.GetState(_gameWindow);
                _spriteBatch.DrawString(_font, "Drawn on UI layer (Camera Moving)", new Vector2(15, _camera.ScreenCenter.Y),
                    mouse.LeftButton == ButtonState.Pressed ? Color.Red : Color.Beige);
            });
        }

        public override void Dispose()
        {

        }
    }
}
