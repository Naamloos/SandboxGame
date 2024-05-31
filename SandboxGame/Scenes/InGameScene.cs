using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Modding;
using SandboxGame.Engine.Scenes;
using SandboxGame.Engine.Storage;
using SandboxGame.Entities;
using SandboxGame.WorldGen;

namespace SandboxGame.Scenes
{
    public class InGameScene : BaseScene
    {
        const string DEV_WORLD_NAME = "default";

        private Player _player;
        private ChatBox _chatBox;
        
        private World _world;
        private WorldInteractionBox _interactionBox;

        private EntityManager _entityManager;
        private LaunchArgs _launchArgs;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private AssetManager _assetManager;
        private IStorageSupplier _storage;
        private ModManager _modManager;

        public InGameScene(EntityManager entityManager, LaunchArgs launchArgs, SpriteBatch spriteBatch, 
            Camera camera, AssetManager assetManager, IStorageSupplier storage, ModManager modManager)
        {
            _entityManager = entityManager;
            _launchArgs = launchArgs;
            _spriteBatch = spriteBatch;
            _camera = camera;
            _assetManager = assetManager;
            _storage = storage;
            _modManager = modManager;
        }

        public override void Initialize()
        {
            _chatBox = _entityManager.SpawnEntity<ChatBox>();
            RegenerateWorld(_launchArgs.ForceNewWorldGen);
        }

        public void RegenerateWorld(bool deleteOld)
        {
            _entityManager.UnloadAllEntities();

            _player = _entityManager.SpawnEntity<Player>();
            _player.SetPosition(PointUnit.Zero);
            _entityManager.SpawnEntity<Npc>().SetPosition(new PointUnit(350, 400));

            if (deleteOld)
            {
                _storage.DeleteWorld(DEV_WORLD_NAME);
            }
            _world = World.Load(DEV_WORLD_NAME, _assetManager, _spriteBatch, _camera, _storage);

            _camera.SetDefaultFollow(_player);
            _camera.Follow(_player);
            _camera.SetSpeed(500);

            _interactionBox = _entityManager.SpawnEntity<WorldInteractionBox>();
            _modManager.WorldLoaded();
        }

        public override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            _interactionBox.Draw();

            _entityManager.DrawEntities();
            _modManager.WorldDraw();
        }

        public override void Update(GameTime gameTime)
        {
            _world.Update(gameTime, _camera);

            _entityManager.UpdateEntities();

            _interactionBox.Update();
            _chatBox.Update();
            _modManager.WorldUpdate();
        }

        public override void Dispose()
        {

        }
    }
}
