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
            // fake fill
            var beginPos = GameContext.Camera.ScreenToWorld(new Vector2(-128, -128));
            var startX = beginPos.X - (beginPos.X % 32);
            var startY = beginPos.Y - (beginPos.Y % 32);
            var endPos = GameContext.Camera.ScreenToWorld(new Vector2(GameContext.GameWindow.ClientBounds.Right + 128, GameContext.GameWindow.ClientBounds.Bottom + 128));
            var endX = endPos.X - (endPos.X % 32);
            var endY = endPos.Y - (endPos.Y % 32);

            for(var x = startX; x < endX; x += 32)
            {
                for(var y = startY; y < endY; y+=32)
                {
                    _grass.Draw(GameContext.SpriteBatch, (int)x, (int)y);
                }
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
