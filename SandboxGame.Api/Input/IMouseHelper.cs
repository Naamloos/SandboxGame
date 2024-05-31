using SandboxGame.Api.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Input
{
    public interface IMouseHelper
    {
        public bool RightButton { get; }
        public bool LeftButton { get; }
        public bool LeftRelease { get; }
        public bool RightRelease { get; }
        public bool LeftClick { get; }
        public bool RightClick { get; }
        public bool ScrollUp { get; }
        public bool ScrollDown { get; }

        public PointUnit ScreenPos { get; }
        public PointUnit WorldPos { get; }
    }
}
