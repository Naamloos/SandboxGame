using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api
{
    public static class ColorHelper
    {
        public static uint RGBA(int r, int g, int b, int a = 0)
        {
            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }
    }
}
