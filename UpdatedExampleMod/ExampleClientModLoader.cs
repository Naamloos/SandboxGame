using Microsoft.Extensions.Logging;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Input;
using UpdatedExampleMod.SoundEffects;
using UpdatedExampleMod.Sprites;

namespace UpdatedExampleMod
{
    public class ExampleClientModLoader : ExampleBaseModLoader, IClientModLoader
    {
        private ILogger logger;
        private IAssetManager assetManager;
        private IKeybindManager keybindManager;
        private IInputHelper inputHelper;

        public ExampleClientModLoader(ILogger<ExampleClientModLoader> logger, IAssetManager assetManager, 
            IKeybindManager keybindManager, IInputHelper inputHelper)
        {
            this.logger = logger;
            this.assetManager = assetManager;
            this.keybindManager = keybindManager;
            this.inputHelper = inputHelper;
        }

        public void OnClientLoad()
        {
            logger.LogInformation("Hello (modded) world!");

            // This is how we can use the asset manager to load our custom assets.
            assetManager.RegisterSprite<ExampleSprite>();
            assetManager.RegisterSoundEffect<ExampleSoundEffect>();

            // This is how we can use the keybind manager to register a keybind.
            keybindManager.RegisterKeybind("updatedexamplemod.bruh", "Bruh Button", 66); // 66 = B,  Keycodes: https://keycode.info/
        }

        public void OnClientUnload()
        {
            logger.LogInformation("Goodbye, (modded) world!");
        }

        public void OnClientWorldDraw()
        {

        }

        public void OnClientWorldLoad()
        {

        }

        public void OnClientWorldTick()
        {
            // This is how we can use the input helper to check if a keybind was pressed.
            // Be wary that this sound effect only plays on the client side!
            if (inputHelper.GetKeyReleased("updatedexamplemod.bruh"))
            {
                logger.LogWarning("Bruh!");
                assetManager.GetSoundEffect<ExampleSoundEffect>().Play(1f);
            }
        }
    }
}
