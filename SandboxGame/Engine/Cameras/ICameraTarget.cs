using Microsoft.Xna.Framework;

namespace SandboxGame.Engine.Cameras
{
    public interface ICameraTarget
    {
        Rectangle Bounds { get; }
    }
}
