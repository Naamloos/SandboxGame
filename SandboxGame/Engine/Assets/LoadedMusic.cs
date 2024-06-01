using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Assets
{
    public class LoadedMusic
    {
        public string Name { get; private set; }
        public TimeSpan Duration => soundEffect.Duration;

        private SoundEffect soundEffect;

        public LoadedMusic(SoundEffect soundEffect)
        {
            this.soundEffect = soundEffect;
        }

        public LoadedMusic(Stream stream)
        {
            SoundEffect.FromStream(stream);
        }

        private SoundEffectInstance instance;
        public void Play()
        {
            instance = soundEffect.CreateInstance();
            instance.Play();
        }

        public void Stop()
        {
            if(instance != null)
                instance.Stop();
        }   
    }
}
