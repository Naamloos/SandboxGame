using Microsoft.Xna.Framework.Audio;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Assets
{
    public class LoadedSoundEffect : ILoadedSoundEffect
    {
        private SoundEffect _soundEffect;
        private ICamera _camera;

        public LoadedSoundEffect(SoundEffect soundEffect, ICamera camera)
        {
            _soundEffect = soundEffect;
            _camera = camera;
        }

        public void Play(float volume)
        {
            var cappedVolume = Math.Min(1, volume);
            _soundEffect.Play(cappedVolume, 0, 0);
        }

        public void PlayAt(float volume, PointUnit worldPosition) // TODO change volume as position gets further from camera, requires update loop
        {
            var cappedVolume = Math.Min(1, volume);
            var distanceFromCamera = PointUnit.Distance(_camera.ScreenCenter, worldPosition);
            var volumeModifier = Math.Max(0, 1 - distanceFromCamera / 1000);
            _soundEffect.Play(cappedVolume * volumeModifier, 0, 0);
        }
    }
}
