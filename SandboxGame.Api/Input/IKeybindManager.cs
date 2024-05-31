using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Input
{
    public interface IKeybindManager
    {
        public void RegisterKeybind(string identifier, string displayName, int defaultKeyCode);
    }
}
