using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Assets
{
    public interface ILoadedSoundEffect
    {
        public void Play(float volume);

        public void PlayAt(float volume, PointUnit worldPosition);
    }
}
