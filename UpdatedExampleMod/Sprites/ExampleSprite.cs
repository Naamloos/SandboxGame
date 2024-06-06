using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedExampleMod.Sprites
{
    /// <summary>
    /// Example of how to define an (animated) sprite asset.
    /// </summary>
    public class ExampleSprite : ISpriteAsset
    {
        public int Frames => 1;

        public Stream GetFrameStream(int frame)
        {
            // Make sure this item is an embedded resource!
            return GetType().Assembly.GetManifestResourceStream("UpdatedExampleMod.EmbeddedAssets.klungo.png")!;
            // For animated sprites, you'll want to load every frame by it's given index. The amount of frames is returned by the Frames property.
            // Spritesheet animations are not supported yet.
        }

        public SpriteMetadata GetMetadata()
        {
            return new SpriteMetadata(16, 16);
        }
    }
}
