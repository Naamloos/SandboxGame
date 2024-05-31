using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using SandboxGame.Api.Assets;

namespace SandboxGame.Engine.Assets
{
    public class AssetManager : IAssetManager
    {
        private ContentManager _contentManager;
        private GraphicsDevice _graphics;
        private GameTimeHelper _gameTime;
        private SpriteBatch _spriteBatch;

        private SpriteFont _fallbackFont;

        private Dictionary<string, LoadedSprite> _sprites = new();
        private Dictionary<string, SpriteFont> _fonts = new();

        public Effect _colorOverlay { get; private set; }

        public AssetManager(ContentManager contentManager, GraphicsDevice gd, GameTimeHelper gameTime, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _graphics = gd;
            _gameTime = gameTime;
            _spriteBatch = spriteBatch;
            Initialize();
        }

        public void Initialize()
        {
            // Pre-load initialization font
            _fallbackFont = _contentManager.Load<SpriteFont>("Fonts/Debug");
            _colorOverlay = _contentManager.Load<Effect>("Shaders/ColorOverlay");

            _fonts.Add("main", _contentManager.Load<SpriteFont>("Fonts/HopeGold"));

            _sprites.Add("water", loadSprite("water", 16, 16, TimeSpan.FromSeconds(.5f)));
            _sprites.Add("player", loadSprite("player", 32, 32, TimeSpan.FromMilliseconds(Random.Shared.Next(2800, 3800))));
            _sprites.Add("grass", loadSprite("grass", 16, 16, TimeSpan.FromSeconds(1)));
            _sprites.Add("dialog", loadSprite("dialog", 16, 16, TimeSpan.FromMilliseconds(500)));
            _sprites.Add("markiplier", loadSprite("markiplier", 32, 32, TimeSpan.FromMilliseconds(Random.Shared.Next(2800, 3800))));
            _sprites.Add("beach_single", loadSprite("beach_single", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("beach_corner", loadSprite("beach_single", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("sand", loadSprite("sand", 32, 32, TimeSpan.FromSeconds(1)));
            _sprites.Add("tree", loadSprite("tree", 32, 32, TimeSpan.FromSeconds(1)));

            _sprites.Add("player_head", loadSprite("player_head", 16, 16, TimeSpan.FromSeconds(3)));
            _sprites.Add("player_hand", loadSprite("player_hand", 8, 8, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_body", loadSprite("player_body", 16, 10, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_foot_l", loadSprite("player_foot_l", 8, 8, TimeSpan.FromSeconds(1)));
            _sprites.Add("player_foot_r", loadSprite("player_foot_r", 8, 8, TimeSpan.FromSeconds(1)));

            _sprites.Add("interact", loadSprite("interact", 32, 32, TimeSpan.FromSeconds(1)));

            _sprites.Add("debug", generateBox(Color.Red));
            _sprites.Add("chat_back", generateBox(Color.Black));
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

        public LoadedSprite GetSprite(string name)
        {
            return _sprites[name].Copy() as LoadedSprite;
        }

        public ILoadedSprite GetSprite<T>() where T : ISpriteAsset
        {
            return _sprites[typeof(T).Name].Copy() as LoadedSprite;
        }

        private LoadedSprite generateBox(Color color)
        {
            var debugBox = new Texture2D(_graphics, 1, 1);
            debugBox.SetData(new[] { color });

            return new LoadedSprite(1,1,TimeSpan.Zero, _colorOverlay, _spriteBatch, _gameTime, debugBox);
        }

        private LoadedSprite loadSprite(string name, int baseWidth, int baseHeight, TimeSpan baseDuration)
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

            return new LoadedSprite(baseWidth, baseHeight, baseDuration, _colorOverlay, _spriteBatch, _gameTime, textures.ToArray());
        }

        public void RegisterSprite<T>() where T : ISpriteAsset
        {
            List<Texture2D> textures = new List<Texture2D>();

            var sprite = Activator.CreateInstance<T>();

            for (int i = 0; i < sprite.Frames; i++)
            {
                textures.Add(Texture2D.FromStream(_graphics, sprite.GetFrame(i)));
            }

            var metaData = sprite.GetMetadata();

            _sprites.Add(typeof(T).Name, new LoadedSprite(metaData.Width, metaData.Height, metaData.AnimationDuration, _colorOverlay, _spriteBatch, _gameTime, textures.ToArray()));
        }
    }
}
