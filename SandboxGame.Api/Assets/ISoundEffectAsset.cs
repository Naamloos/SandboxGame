using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Assets
{
    public interface ISoundEffectAsset
    {
        public Stream GetStream();
    }
}
