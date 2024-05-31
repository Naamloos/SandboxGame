using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.Assets
{
    public class SomeCoolSprite : ISpriteAsset
    {
        public int Frames => 1;

        public Stream GetFrame(int frame)
        {
            // Make sure this item is an embedded resource!
            return this.GetType().Assembly.GetManifestResourceStream("ExampleMod.Assets.klungo.png")!;
        }

        public SpriteMetadata GetMetadata()
        {
            return new SpriteMetadata(16, 16);
        }
    }
}
