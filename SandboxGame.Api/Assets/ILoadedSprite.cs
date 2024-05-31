using SandboxGame.Api.Camera;
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

        public void Update();

        public void Draw(int x, int y, bool interactable = false, bool flip = false,
            ICamera camera = null, uint? lightColor = null, int widthOverride = -1, int heightOverride = -1, float rotation = 0);
    }
}
