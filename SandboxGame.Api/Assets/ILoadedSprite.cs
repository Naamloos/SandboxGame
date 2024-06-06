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

        public void Draw(RectangleUnit destination, bool interactable = false, bool flip = false,
            uint? lightColor = null, float rotation = 0);
    }
}
