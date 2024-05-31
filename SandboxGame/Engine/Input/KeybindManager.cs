using Microsoft.Xna.Framework.Input;
using SandboxGame.Api.Input;
using SandboxGame.Engine.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Input
{
    // TODO Optimize?
    // TODO Implement rebinding
    public class KeybindManager : IKeybindManager
    {
        private Dictionary<string, KeybindInfo> keybinds = new();
        private IStorageSupplier _storageSupplier;

        public KeybindManager(IStorageSupplier storageSupplier)
        {
            _storageSupplier = storageSupplier;
            LoadKeybinds();
        }

        public void LoadKeybinds()
        {
            // Preload required defaults
            keybinds.Add("up", new KeybindInfo("up", "Move Up", Keys.W));
            keybinds.Add("down", new KeybindInfo("down", "Move Down", Keys.S));
            keybinds.Add("left", new KeybindInfo("left", "Move Left", Keys.A));
            keybinds.Add("right", new KeybindInfo("right", "Move Right", Keys.D));
            keybinds.Add("debug", new KeybindInfo("debug", "Show Debug Info", Keys.F1));
            keybinds.Add("interact", new KeybindInfo("interact", "Interact", Keys.E));

            // load binds from file, override the defaults
            var loadedKeybinds = _storageSupplier.ReadConfigFile<Dictionary<string, KeybindInfo>>("keybinds");
            foreach (var bind in loadedKeybinds)
            {
                if (keybinds.ContainsKey(bind.Key))
                {
                    keybinds[bind.Key] = bind.Value;
                }
                else
                {
                    keybinds.Add(bind.Key, bind.Value);
                }
            }

            // re-save file
            _storageSupplier.StoreConfigFile("keybinds", keybinds);
        }

        public void RegisterKeybind(string identifier, string displayName, int defaultKeyCode)
        {
            if(keybinds.ContainsKey(identifier))
            {
                return;
            }
            keybinds.Add(identifier, new KeybindInfo(identifier, displayName, (Keys)defaultKeyCode));
            _storageSupplier.StoreConfigFile("keybinds", keybinds);
        }

        public int GetKeyCode(string identifier) => (int)keybinds[identifier].CurrentKey;

        public Keys GetKey(string identifier) => keybinds[identifier].CurrentKey;
    }

    public class KeybindInfo
    {
        public string Identifier { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public Keys DefaultKey { get; set; }
        public Keys CurrentKey { get; set; }

        public KeybindInfo()
        {
            Identifier = string.Empty;
            DisplayName = string.Empty;
            Category = string.Empty;
            DefaultKey = Keys.None;
            CurrentKey = Keys.None;
        }

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
