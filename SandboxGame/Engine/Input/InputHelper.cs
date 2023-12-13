using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Input
{
    public class InputHelper
    {
        public bool Up { get; private set; }
        public bool Down { get; private set; }
        public bool Left { get; private set; }
        public bool Right { get; private set; }

        public bool Interact { get; private set; }

        public InputHelper()
        {

        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();

            Up = keyboardState.IsKeyDown(Keys.W);
            Down = keyboardState.IsKeyDown(Keys.S);
            Left = keyboardState.IsKeyDown(Keys.A);
            Right = keyboardState.IsKeyDown(Keys.D);
            Interact = keyboardState.IsKeyDown(Keys.E);

            DebugHelper.SetDebugValues("INPUT", $"U {Up} D {Down} L {Left} R {Right} I {Interact}");
        }
    }
}
