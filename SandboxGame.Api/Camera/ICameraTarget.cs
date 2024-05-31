using SandboxGame.Api.Units;

namespace SandboxGame.Api.Camera
{
    public interface ICameraTarget
    {
        RectangleUnit Bounds { get; }
    }
}
