using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Modding;
using SandboxGame.Engine.Scenes;
using SandboxGame.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame.Engine.DependencyInjection
{
    public class GameHostService : IHostedService
    {
        private Game _game;
        private Camera _camera;
        private SceneManager _sceneManager;
        private SpriteBatch _spriteBatch;
        private AssetManager _assetManager;
        private SpriteFont _debugTextFont;
        private MouseHelper _mouseHelper;
        private InputHelper _inputHelper;
        private GameTimeHelper _gameTimeHelper;
        private ModManager _modManager;
        private ILogger _logger;

        private bool debugInfo = false;

        public GameHostService(Camera camera, SceneManager sceneManager, 
            SpriteBatch spriteBatch, AssetManager assetManager, MouseHelper mouseHelper, 
            InputHelper inputHelper, GameTimeHelper gameTimeHelper,
            ILogger<GameHostService> logger)
        {
            _logger = logger;

            _camera = camera;
            _sceneManager = sceneManager;
            _spriteBatch = spriteBatch;
            _assetManager = assetManager;
            _debugTextFont = assetManager.GetFont();
            _mouseHelper = mouseHelper;
            _inputHelper = inputHelper;
            _gameTimeHelper = gameTimeHelper;

            _game = Program.Game;
            _game.Exiting += gameExiting;

            _game.OnUpdate += onUpdate;
            _game.OnDraw += onDraw;
            _spriteBatch = spriteBatch;

            _sceneManager.Switch<InGameScene>();
        }

        private void onDraw(GameTime gameTime)
        {
            DebugHelper.SetDebugValues("FRAMERATE", Math.Round((1.0f / gameTime.ElapsedGameTime.TotalSeconds)).ToString());
            _game.GraphicsDevice.Clear(Color.Black);

            // Camera layer
            _camera.DrawOnCamera(() =>
            {
                _sceneManager.Draw(gameTime);
            });

            // UI layer
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap);
            if (debugInfo)
            {
                var dbg = DebugHelper.GetDebugString();
                _spriteBatch.DrawString(_debugTextFont, dbg, new Vector2(10, 10), Color.YellowGreen);
            }
            _camera.FlushUIDraw();
            _spriteBatch.End();
        }

        private void onUpdate(GameTime gameTime)
        {
            _gameTimeHelper.Update(gameTime);
            _sceneManager.Update(gameTime);
            _camera.Update(gameTime);
            _mouseHelper.Update();

            _inputHelper.Update();

            if (_inputHelper.GetKeyPressed("debug"))
            {
                debugInfo = !debugInfo;
            }
        }

        private void gameExiting(object sender, EventArgs e)
        {
            StopAsync(new CancellationToken());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
