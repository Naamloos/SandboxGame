using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.Entities;
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
        private Npc _npc;
        private Sprite _grass;

        public override void Initialize(GameContext gameContext)
        {
            _gameContext = gameContext;

            var playerSprite = gameContext.AssetManager.GetSprite("player");

            _player = new Player(playerSprite, gameContext.InputHelper, gameContext.Camera);
            _npc = new Npc(playerSprite, gameContext.Camera, new Vector2(350, 400), gameContext.MouseHelper);

            _grass = gameContext.AssetManager.GetSprite("grass");

            _gameContext.Camera.Follow(_player);
            _gameContext.Camera.SetSpeed(500);
        }

        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < (15 * 15); i++)
            {
                int x = i % 15;
                int y = (i - x) / 15;

                _grass.Draw(_gameContext.SpriteBatch, x * 32, y * 32);
            }

            _npc.Draw(_gameContext.SpriteBatch);
            _player.Draw(_gameContext.SpriteBatch);
        }

        public override void DrawUI(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _grass.Update(gameTime);

            _player.Update(gameTime);
            _npc.Update(gameTime);
        }

        public override void Dispose()
        {
        }
    }
}
