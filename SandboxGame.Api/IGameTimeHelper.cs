using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public interface IGameTimeHelper
    {
        public TimeSpan ElapsedGameTime { get; }

        public TimeSpan TotalGameTime { get; }

        public bool IsRunningSlowly { get; }
    }
}
