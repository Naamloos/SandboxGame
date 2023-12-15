using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Scenes;
using SandboxGame.Entities;

namespace SandboxGame.Scenes
{
    public class DevScene : BaseScene
    {
        private Player _player;
        private Npc _npc;
        private Sprite _grass;

        public override void Initialize()
        {
            _player = new Player(GameContext);
            _npc = new Npc(GameContext, "markiplier", new Vector2(350, 400));

            _grass = GameContext.AssetManager.GetSprite("grass");

            GameContext.Camera.SetDefaultFollow(_player);
            GameContext.Camera.SetSpeed(500);
        }

        public override void Draw(GameTime gameTime)
        {
            for(int i = 0; i < (15 * 15); i++)
            {
                int x = i % 15;
                int y = (i - x) / 15;

                _grass.Draw(GameContext.SpriteBatch, x * 32, y * 32);
            }

            _npc.Draw(GameContext.SpriteBatch);
            _player.Draw(GameContext.SpriteBatch);
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
