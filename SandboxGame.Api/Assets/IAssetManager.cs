using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Assets
{
    public interface IAssetManager
    {
        public void RegisterSprite<T>() where T : ISpriteAsset;

        public ILoadedSprite GetSprite<T>() where T : ISpriteAsset;
    }
}
