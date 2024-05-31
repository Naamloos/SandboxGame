using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxGame.Api.Units;

namespace SandboxGame.Api.Camera
{
    public interface ICamera
    {
        RectangleUnit WorldView { get; }
        RectangleUnit ScreenView { get; }
        PointUnit ScreenCenter { get; }
        bool IsFollowing { get; }
        ICameraTarget Target { get; }
        float Zoom { get; }

        public void DrawOnCamera(Action draw);
    }
}
