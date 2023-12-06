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

        public override void Initialize(GameContext gameContext)
        {
            _gameContext = gameContext;
        }

        public override void Draw(GameTime gameTime)
        {
            _gameContext.SpriteBatch.GraphicsDevice.Clear(Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Dispose()
        {

        }
    }
}
