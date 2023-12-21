using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Engine.Cameras;

namespace SandboxGame.Engine.Input
{
    public class MouseHelper
    {
        public bool RightButton { get; private set; }
        public bool LeftButton { get; private set; }
        public bool LeftRelease { get; private set; }
        public bool RightRelease { get; private set; }
        public bool LeftClick { get; private set; }
        public bool RightClick { get; private set; }
        public bool ScrollUp { get; private set; }
        public bool ScrollDown { get; private set; }

        public Vector2 ScreenPos { get; private set; }
        public Vector2 WorldPos { get; private set; }

        private GameWindow _window;
        private Camera _camera;
        public MouseHelper(GameWindow gameWindow, Camera camera)
        {
            _window = gameWindow;
            _camera = camera;

            // grab initial scroll
            previousScroll = Mouse.GetState(gameWindow).ScrollWheelValue;
        }

        private bool previousLeftState = false;
        private bool previousRightState = false;
        private int previousScroll = 0;

        public void Update()
        {
            var state = Mouse.GetState(_window);

            ScreenPos = state.Position.ToVector2();
            WorldPos = _camera.ScreenToWorld(ScreenPos);

            RightButton = state.RightButton == ButtonState.Pressed;
            LeftButton = state.LeftButton == ButtonState.Pressed;

            LeftRelease = previousLeftState && !LeftButton;
            RightRelease = previousRightState && !RightButton;

            LeftClick = !previousLeftState && LeftButton;
            RightClick = !previousRightState && RightButton;

            ScrollUp = previousScroll < state.ScrollWheelValue;
            ScrollDown = previousScroll > state.ScrollWheelValue;

            previousLeftState = LeftButton;
            previousRightState = RightButton;
            previousScroll = state.ScrollWheelValue;

            DebugHelper.SetDebugValues("MOUSE", $"L {LeftButton} R {RightButton} LC {LeftClick} RC {RightClick} " +
                $"LR {LeftRelease} RR {RightRelease} _lp {previousLeftState} _rp {previousRightState}");

            DebugHelper.SetDebugValues("MOUSE_SCREEN_POS", $"X: {ScreenPos.X.ToString().PadRight(5)} Y: {ScreenPos.Y.ToString().PadRight(5)}");
            DebugHelper.SetDebugValues("MOUSE_WORLD_POS", $"X: {WorldPos.X.ToString().PadRight(5)} Y: {WorldPos.Y.ToString().PadRight(5)}");
        }
    }
}
