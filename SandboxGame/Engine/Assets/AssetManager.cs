using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace SandboxGame.Engine.Assets
{
    public class AssetManager
    {
        private ContentManager _contentManager;
        private GraphicsDevice _graphics;

        private SpriteFont _fallbackFont;

        private Dictionary<string, Sprite> _sprites = new();
        private Dictionary<string, SpriteFont> _fonts = new();

        public Effect _colorOverlay { get; private set; }

        public AssetManager(ContentManager contentManager, GraphicsDevice gd)
        {
            _contentManager = contentManager;
            _graphics = gd;
        }

        public void Initialize()
        {
            // Pre-load initialization font
            _fallbackFont = _contentManager.Load<SpriteFont>("Fonts/Debug");
            _colorOverlay = _contentManager.Load<Effect>("Shaders/ColorOverlay");

            _fonts.Add("main", _contentManager.Load<SpriteFont>("Fonts/HopeGold"));

            _sprites.Add("water", loadSprite("water", 32, 32, TimeSpan.FromSeconds(.5f)));
            _sprites.Add("player", loadSprite("player", 32, 32, TimeSpan.FromMilliseconds(Random.Shared.Next(2800, 3800))));
            _sprites.Add("grass", loadSprite("grass", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("dialog", loadSprite("dialog", 16, 16, TimeSpan.FromMilliseconds(500)));
            _sprites.Add("markiplier", loadSprite("markiplier", 32, 32, TimeSpan.FromMilliseconds(Random.Shared.Next(2800, 3800))));
            _sprites.Add("beach_single", loadSprite("beach_single", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("beach_corner", loadSprite("beach_single", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("sand", loadSprite("sand", 32, 32, TimeSpan.FromSeconds(1)));

            _sprites.Add("player_head", loadSprite("player_head", 16, 16, TimeSpan.FromSeconds(3)));
            _sprites.Add("player_hand", loadSprite("player_hand", 8, 8, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_body", loadSprite("player_body", 16, 10, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_foot_l", loadSprite("player_foot_l", 8, 8, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_foot_r", loadSprite("player_foot_r", 8, 8, TimeSpan.FromSeconds(1)));

            _sprites.Add("interact", loadSprite("interact", 32, 32, TimeSpan.FromSeconds(1)));

            _sprites.Add("debug", generateBox());
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
            return _sprites[name].Copy();
        }

        private Sprite generateBox()
        {
            var debugBox = new Texture2D(_graphics, 1, 1);
            debugBox.SetData(new[] { Color.Red });

            return new Sprite(1,1,TimeSpan.Zero, _colorOverlay, debugBox);
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
                }
                catch(Exception ex)
                {
                    break;
                }
            }

            return new Sprite(baseWidth, baseHeight, baseDuration, _colorOverlay, textures.ToArray());
        }
    }
}
