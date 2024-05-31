using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.Sprites
{
    public class KlungoSprite : ISpriteAsset
    {
        public int Frames => 1;

        public Stream GetFrameStream(int frame)
        {
            // Make sure this item is an embedded resource!
            return GetType().Assembly.GetManifestResourceStream("ExampleMod.Assets.klungo.png")!;
            // For animated sprites, you'll want to load every frame by it's given index. The amount of frames is returned by the Frames property.
        }

        public SpriteMetadata GetMetadata()
        {
            return new SpriteMetadata(16, 16);
        }
    }
}
