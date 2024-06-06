using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatedExampleMod.SoundEffects
{
    /// <summary>
    /// Example of how to define a sound effect asset.
    /// </summary>
    public class ExampleSoundEffect : ISoundEffectAsset
    {
        public Stream GetStream()
        {
            // this HAS to be a wav file!!!
            return GetType().Assembly.GetManifestResourceStream("UpdatedExampleMod.EmbeddedAssets.bruh.wav")!;
        }
    }
}
