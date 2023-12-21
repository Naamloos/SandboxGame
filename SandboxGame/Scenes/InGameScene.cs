using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.Engine.Scenes;
using SandboxGame.Entities;
using SandboxGame.WorldGen;

namespace SandboxGame.Scenes
{
    public class InGameScene : BaseScene
    {
        private Player _player;
        private Npc _npc;
        
        private World _world;
        private WorldInteractionBox _interactionBox;

        public override void Initialize()
        {
            _player = new Player(GameContext);
            _npc = new Npc(GameContext, "markiplier", new Vector2(350, 400));

            _world = World.Load("my world", GameContext);

            GameContext.Camera.SetDefaultFollow(_player);
            GameContext.Camera.SetSpeed(500);


            _interactionBox = new WorldInteractionBox(GameContext);
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            _interactionBox.Draw(gameTime);
            _npc.Draw(GameContext.SpriteBatch);
            _player.Draw(GameContext.SpriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _interactionBox.Update(gameTime);
        }

        public override void Dispose()
        {
        }
    }
}
