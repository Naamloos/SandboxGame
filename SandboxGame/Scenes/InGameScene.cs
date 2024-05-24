using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
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

        private EntityManager _entityManager;
        private LaunchArgs _launchArgs;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private AssetManager _assetManager;
        private MouseHelper _mouseHelper;

        public InGameScene(EntityManager entityManager, LaunchArgs launchArgs, SpriteBatch spriteBatch, 
            Camera camera, AssetManager assetManager, MouseHelper mouseHelper)
        {
            _entityManager = entityManager;
            _launchArgs = launchArgs;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _assetManager = assetManager;
        }

        public override void Initialize()
        {
            _player = _entityManager.SpawnEntity<Player>();
            _npc = _entityManager.SpawnEntity<Npc>();
            _npc.SetPosition(new Vector2(350, 400));

            _world = World.Load("my world", _launchArgs.ForceNewWorldGen, _assetManager, _spriteBatch, _camera);

            _camera.SetDefaultFollow(_player);
            _camera.SetSpeed(500);


            _interactionBox = _entityManager.SpawnEntity<WorldInteractionBox>();
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            _interactionBox.Draw(_spriteBatch);
            _npc.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime, _camera);
            _player.Update(gameTime);
            _npc.Update(gameTime);
            _interactionBox.Update(gameTime);
        }

        public override void Dispose()
        {

        }
    }
}
