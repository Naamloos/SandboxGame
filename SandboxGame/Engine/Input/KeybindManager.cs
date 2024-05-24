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
        private GameContext gameContext;

        public KeybindManager(GameContext ctx)
        {
            gameContext = ctx;
        }

        public void LoadKeybinds()
        {
            // Preload required defaults
            keybinds.Add("up", new KeybindInfo("up", "Move Up", Keys.W));
            keybinds.Add("down", new KeybindInfo("down", "Move Down", Keys.S));
            keybinds.Add("left", new KeybindInfo("left", "Move Left", Keys.A));
            keybinds.Add("right", new KeybindInfo("right", "Move Right", Keys.D));
            keybinds.Add("debug", new KeybindInfo("debug", "Show Debug Info", Keys.F1));
            keybinds.Add("interact", new KeybindInfo("e", "Interact", Keys.E));

            // load binds from file, override the defaults
            var loadedKeybinds = gameContext.StorageSupplier.ReadConfigFile<Dictionary<string, KeybindInfo>>("keybinds");
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
            gameContext.StorageSupplier.StoreConfigFile("keybinds", keybinds);
        }

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
