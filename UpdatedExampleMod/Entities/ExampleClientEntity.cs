using SandboxGame.Api.Assets;
using SandboxGame.Api.Attributes;
using SandboxGame.Api.Entity;
using UpdatedExampleMod.Sprites;

namespace UpdatedExampleMod.Entities
{
    /// <summary>
    /// This is an example of the Client entity implementation. 
    /// This code is specifically for execution on the server side.
    /// </summary>
    [EntityImplementation("example", 32, 32)]
    public class ExampleClientEntity : BaseClientEntity
    {
        private ILoadedSprite loadedSprite;

        public ExampleClientEntity(IAssetManager assetManager)
        {
            this.loadedSprite = assetManager.GetSprite<ExampleSprite>();
        }

        public override void OnClientDraw()
        {
            loadedSprite.Draw(Bounds, this.IsInteractable);
        }

        public override void OnClientTick()
        {
            loadedSprite.Update();
        }

        public override void OnClientClick()
        {
            // Executed on client click (this entity was clicked in the client)
        }
    }
}
