using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public interface IMod
    {
        public ModMetadata GetMetadata();

        public void OnLoad();

        public void OnUnload();

        public void OnWorldLoaded();

        public void OnWorldDraw();

        public void OnWorldUpdate();
    }
}
