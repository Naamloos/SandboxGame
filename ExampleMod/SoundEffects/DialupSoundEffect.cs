using SandboxGame.Api.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleMod.SoundEffects
{
    public class DialupSoundEffect : ISoundEffectAsset
    {
        public Stream GetStream()
        {
            return GetType().Assembly.GetManifestResourceStream("ExampleMod.Assets.dialup-internet.mp3")!;
        }
    }
}
