using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.SoundEffects
{
    public class BruhSoundEffect : ISoundEffectAsset
    {
        public Stream GetStream()
        {
            // this HAS to be a wav file!!!
            return GetType().Assembly.GetManifestResourceStream("ExampleMod.Assets.bruh.wav")!;
        }
    }
}
