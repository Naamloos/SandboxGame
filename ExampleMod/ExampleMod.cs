using ExampleMod.Assets;
using SandboxGame.Api;
using SandboxGame.Api.Assets;

namespace ExampleMod
{
    public class ExampleMod : IMod
    {
        private IAssetManager assetManager;

        public ExampleMod(IAssetManager assetManager)
        {
            // dependency injection!
            this.assetManager = assetManager;
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
            assetManager.RegisterSprite<SomeCoolSprite>(); // this is our cool sprite :3
            // I recommend actually doing this in the OnLoad method of your mod, since the game may break loading assets elsewhere. Not sure though.
        }

        public void OnUnload()
        {
            Console.WriteLine("Goodbye, world!");
        }
    }
}
