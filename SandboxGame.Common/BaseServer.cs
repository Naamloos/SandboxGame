using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common
{
    public abstract class BaseServer
    {
        public abstract Task StartAsync();

        public abstract Task StopAsync();

        public async Task TickAsync()
        {

        }
    }
}
