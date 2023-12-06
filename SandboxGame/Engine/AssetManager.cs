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
    internal class AssetManager
    {
        private ContentManager _contentManager;

        private string _fallbackFontName;
        private Dictionary<string, SpriteFont> _fonts = new();

        public AssetManager(string fallbackFontName, ContentManager contentManager)
        {
            _contentManager = contentManager;
            _fallbackFontName = fallbackFontName;
        }

        public void Initialize()
        {
            _fonts.Add(_fallbackFontName, _contentManager.Load<SpriteFont>(_fallbackFontName));
        }

        public void LoadFont(string name)
        {
            if(_fonts.ContainsKey(name))
            {
                throw new Exception($"Font with name {name} was already loaded!");
            }

            _fonts.Add(name, _contentManager.Load<SpriteFont>(name));
        }

        public SpriteFont GetFont(string name = "")
        {
            if(_fonts.ContainsKey(name))
            {
                return _fonts[name];
            }

            return _fonts[_fallbackFontName];
        }
    }
}
