using ExampleMod.Entities;
using ExampleMod.SoundEffects;
using ExampleMod.Sprites;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Entity;

namespace ExampleMod
{
    public class ExampleMod : IMod
    {
        private IAssetManager assetManager;
        private IEntityManager entityManager;

        public ExampleMod(IAssetManager assetManager, IEntityManager entityManager)
        {
            // dependency injection!
            this.assetManager = assetManager;
            this.entityManager = entityManager;
        }

        public ModMetadata GetMetadata()
        {
            return new ModMetadata()
            {
                Name = "Example Mod",
                Author = "Naamloos",
                Version = "1.0.0",
                Description = "An example mod for SandboxGame."
            };
        }

        public void OnLoad()
        {
            Console.WriteLine("ExampleMod loading assets!");
            assetManager.RegisterSprite<KlungoSprite>(); // this is our cool sprite :3
            assetManager.RegisterSoundEffect<DialupSoundEffect>();
            // I recommend actually doing this in the OnLoad method of your mod, since the game may break loading assets elsewhere. Not sure though.
        }

        public void OnUnload()
        {
            Console.WriteLine("Goodbye, world!");
        }

        public void OnWorldLoaded()
        {
            entityManager.SpawnEntity<KlungoEntity>(); // this is our cool entity :3
            assetManager.GetSoundEffect<DialupSoundEffect>().Play(1f);
        }
    }
}
