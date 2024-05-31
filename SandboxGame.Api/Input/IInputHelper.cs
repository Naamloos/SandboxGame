using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Input
{
    public interface IInputHelper
    {
        public bool GetKeyDown(string identifier);

        public bool GetKeyUp(string identifier);

        public bool GetKeyPressed(string identifier);

        public bool GetKeyReleased(string identifier);
    }
}
