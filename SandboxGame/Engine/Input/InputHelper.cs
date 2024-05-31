using Microsoft.Xna.Framework.Input;
using SandboxGame.Api.Input;
using System.Collections.Generic;

namespace SandboxGame.Engine.Input
{
    public class InputHelper : IInputHelper
    {
        private List<Keys> currentlyHeldKeys = new List<Keys>();
        private List<Keys> previouslyHeldKeys = new List<Keys>();

        private KeybindManager _keybindManager;

        public InputHelper(KeybindManager keybindManager)
        {
            _keybindManager = keybindManager;
        }

        public bool GetKeyDown(string identifier) => GetKeyDown(_keybindManager.GetKey(identifier));
        public bool GetKeyDown(Keys key)
        {
            return currentlyHeldKeys.Contains(key);
        }

        public bool GetKeyUp(string identifier) => GetKeyUp(_keybindManager.GetKey(identifier));
        public bool GetKeyUp(Keys key)
        {
            return !currentlyHeldKeys.Contains(key);
        }

        public bool GetKeyPressed(string identifier) => GetKeyPressed(_keybindManager.GetKey(identifier));
        public bool GetKeyPressed(Keys key)
        {
            return currentlyHeldKeys.Contains(key) && !previouslyHeldKeys.Contains(key);
        }

        public bool GetKeyReleased(string identifier) => GetKeyReleased(_keybindManager.GetKey(identifier));
        public bool GetKeyReleased(Keys key)
        {
            return !currentlyHeldKeys.Contains(key) && previouslyHeldKeys.Contains(key);
        }

        public void Update()
        {
            var keyboardState = Keyboard.GetState();

            previouslyHeldKeys.Clear();
            previouslyHeldKeys.AddRange(currentlyHeldKeys);
            previouslyHeldKeys.RemoveAll(x => x == Keys.None);
            currentlyHeldKeys.Clear();
            currentlyHeldKeys.AddRange(keyboardState.GetPressedKeys());
            currentlyHeldKeys.RemoveAll(x => x == Keys.None);
        }
    }
}
