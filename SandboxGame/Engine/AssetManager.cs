using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace SandboxGame.Engine
{
    public class AssetManager
    {
        private ContentManager _contentManager;

        private SpriteFont _fallbackFont;

        private Dictionary<string, Sprite> _sprites = new();
        private Dictionary<string, SpriteFont> _fonts = new();

        public Effect _colorOverlay { get; private set; }

        public AssetManager(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Initialize()
        {
            // Pre-load initialization font
            _fallbackFont = _contentManager.Load<SpriteFont>("Fonts/Debug");
            _colorOverlay = _contentManager.Load<Effect>("Shaders/ColorOverlay");

            _fonts.Add("main", _contentManager.Load<SpriteFont>("Fonts/HopeGold"));

            _sprites.Add("tile", loadSprite("tile", 64, 64, TimeSpan.FromSeconds(.5f)));
            _sprites.Add("player", loadSprite("player", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("grass", loadSprite("grass", 32, 32, TimeSpan.FromSeconds(1)));
        }

        public SpriteFont GetFont(string name = "")
        {
            if(string.IsNullOrEmpty(name))
                return _fallbackFont;

            try
            {
                return _fonts[name];
            }
            catch(Exception ex)
            {
                return _fallbackFont;
            }
        }

        public Sprite GetSprite(string name)
        {
            return _sprites[name];
        }

        private Sprite loadSprite(string name, int baseWidth, int baseHeight, TimeSpan baseDuration)
        {
            int i = 0;
            List<Texture2D> textures = new List<Texture2D>();

            while(true)
            {
                try
                {
                    textures.Add(_contentManager.Load<Texture2D>($"Sprites/{name}" + (i > 0? $"_{i}" : "")));
                    i++;
                }catch(Exception ex)
                {
                    break;
                }
            }

            return new Sprite(baseWidth, baseHeight, baseDuration, _colorOverlay, textures.ToArray());
        }
    }
}
