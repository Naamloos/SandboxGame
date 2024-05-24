using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SandboxGame.Engine.Input
{
    public class InputHelper
    {
        private List<Keys> currentlyHeldKeys = new List<Keys>();
        private List<Keys> previouslyHeldKeys = new List<Keys>();
        private GameContext gameContext;

        public InputHelper(GameContext context)
        {
            gameContext = context;
        }

        public bool GetKeyDown(string identifier) => GetKeyDown(gameContext.KeybindManager.GetKey(identifier));
        public bool GetKeyDown(Keys key)
        {
            return currentlyHeldKeys.Contains(key);
        }

        public bool GetKeyUp(string identifier) => GetKeyUp(gameContext.KeybindManager.GetKey(identifier));
        public bool GetKeyUp(Keys key)
        {
            return !currentlyHeldKeys.Contains(key);
        }

        public bool GetKeyPressed(string identifier) => GetKeyPressed(gameContext.KeybindManager.GetKey(identifier));
        public bool GetKeyPressed(Keys key)
        {
            return currentlyHeldKeys.Contains(key) && !previouslyHeldKeys.Contains(key);
        }

        public bool GetKeyReleased(string identifier) => GetKeyReleased(gameContext.KeybindManager.GetKey(identifier));
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
