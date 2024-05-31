using SandboxGame.Api.Units;

namespace SandboxGame.Api.Assets
{
    public interface ILoadedSprite
    {
        public int Width { get; }
        public int Height { get; }
        public RectangleUnit Bounds { get; }

        public void SetDuration(TimeSpan duration);
        public ILoadedSprite Copy();
    }
}
