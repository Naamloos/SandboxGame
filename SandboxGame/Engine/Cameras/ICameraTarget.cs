using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Cameras
{
    public interface ICameraTarget
    {
        Rectangle Bounds { get; }
    }
}
