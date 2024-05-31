using ExampleMod.Entities;
using ExampleMod.SoundEffects;
using ExampleMod.Sprites;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Input;

namespace ExampleMod
{
    public class ExampleMod : IMod
    {
        private IAssetManager assetManager;
        private IEntityManager entityManager;
        private IKeybindManager keybindManager;
        private IInputHelper inputHelper;

        public ExampleMod(IAssetManager assetManager, IEntityManager entityManager, IKeybindManager keybindManager, IInputHelper inputHelper)
        {
            // dependency injection!
            this.assetManager = assetManager;
            this.entityManager = entityManager;
            this.keybindManager = keybindManager;
            this.inputHelper = inputHelper;
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
            assetManager.RegisterSoundEffect<BruhSoundEffect>();
            // I recommend actually doing this in the OnLoad method of your mod, since the game may break loading assets elsewhere. Not sure though.

            keybindManager.RegisterKeybind("examplemod.bruh", "Certified Based Bruh Key", 66); // 66 = B,  Keycodes: https://keycode.info/
        }

        public void OnUnload()
        {
            Console.WriteLine("Goodbye, world!");
        }

        public void OnWorldDraw()
        {

        }

        public void OnWorldLoaded()
        {
            entityManager.SpawnEntity<KlungoEntity>(); // this is our cool entity :3
        }

        public void OnWorldUpdate()
        {
            if(inputHelper.GetKeyReleased("examplemod.bruh"))
            {
                Console.WriteLine("Bruh moment");
                assetManager.GetSoundEffect<BruhSoundEffect>().Play(1f);
            }
        }
    }
}
