using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Scenes;
using SandboxGame.Entities;
using SandboxGame.World;

namespace SandboxGame.Scenes
{
    public class DevScene : BaseScene
    {
        private Player _player;
        private Npc _npc;
        
        private WorldManager _chunkManager;
        private WorldInteractionBox _interactionBox;

        public override void Initialize()
        {
            _player = new Player(GameContext);
            _npc = new Npc(GameContext, "markiplier", new Vector2(350, 400));

            _chunkManager = new WorldManager(GameContext);

            GameContext.Camera.SetDefaultFollow(_player);
            GameContext.Camera.SetSpeed(500);


            _interactionBox = new WorldInteractionBox(GameContext);
        }

        public override void Draw(GameTime gameTime)
        {
            _chunkManager.Draw(gameTime);
            _interactionBox.Draw(gameTime);
            _npc.Draw(GameContext.SpriteBatch);
            _player.Draw(GameContext.SpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _chunkManager.Update(gameTime);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _interactionBox.Update(gameTime);
        }

        public override void Dispose()
        {
        }
    }
}
