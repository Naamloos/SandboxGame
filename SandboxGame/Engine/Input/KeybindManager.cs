using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Input
{
    // TODO Optimize?
    // TODO Implement rebinding
    public class KeybindManager
    {
        private Dictionary<string, KeybindInfo> keybinds = new();

        public KeybindManager() 
        {
            
        }

        public void LoadKeybinds()
        {
            keybinds.Add("up", new KeybindInfo("up", "Move Up", Keys.W));
            keybinds.Add("down", new KeybindInfo("down", "Move Down", Keys.S));
            keybinds.Add("left", new KeybindInfo("left", "Move Left", Keys.A));
            keybinds.Add("right", new KeybindInfo("right", "Move Right", Keys.D));
            keybinds.Add("debug", new KeybindInfo("debug", "Show Debug Info", Keys.F1));
            keybinds.Add("interact", new KeybindInfo("e", "Interact", Keys.E));
        }

        public Keys GetKey(string identifier) => keybinds[identifier].CurrentKey;
    }

    public struct KeybindInfo
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public Keys DefaultKey { get; set; }
        public Keys CurrentKey { get; set; }

        public KeybindInfo(string identifier, string displayname, Keys defaultKey, Keys? currentKey = null, string category = null)
        {
            Category = category ?? "Game";
            Identifier = identifier;
            DisplayName = displayname;
            DefaultKey = defaultKey;
            CurrentKey = currentKey ?? DefaultKey;
        }
    }
}
