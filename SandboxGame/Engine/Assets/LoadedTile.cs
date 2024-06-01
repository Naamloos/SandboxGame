using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using ProtoBuf.WellKnownTypes;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxGame.Api.Input;
using SandboxGame.Engine.Cameras;

namespace SandboxGame.Engine.Assets
{
    public class LoadedTile : ILoadedSprite
    {
        public int Width { get; private set; }

        public int Height { get; private set; }

        public RectangleUnit Bounds { get; private set; }

        private bool hovering;

        private MouseHelper _mouseHelper;
        private GameTimeHelper _gameTimeHelper;
        private SpriteBatch _spriteBatch;
        private Effect _colorOverlay;

        private PointUnit tilePosition;
        private Texture2D tileSet;

        private Rectangle sourceRectangle;

        public LoadedTile(int width, int height, Effect colorOverlay,
            SpriteBatch spriteBatch, GameTimeHelper gameTimeHelper, MouseHelper mouseHelper,  Texture2D tileSet, PointUnit tilePosition)
        {
            Width = width;
            Height = height;
            _colorOverlay = colorOverlay;
            _spriteBatch = spriteBatch;
            _gameTimeHelper = gameTimeHelper;
            _mouseHelper = mouseHelper;
            this.tileSet = tileSet;
            this.tilePosition = tilePosition;
            sourceRectangle = new Rectangle((int)tilePosition.X, (int)tilePosition.Y, width, height);
        }

        public ILoadedSprite Copy()
        {
            return new LoadedTile(Width, Height, _colorOverlay, _spriteBatch, _gameTimeHelper, _mouseHelper, tileSet, tilePosition);
        }

        public void Draw(int x, int y, bool interactable = false, bool flip = false,
            ICamera camera = null, uint? lightColor = null, int widthOverride = -1, int heightOverride = -1, float rotation = 0)
        {
            Bounds = new RectangleUnit(x, y, Width, Height);

            int width = widthOverride > 0 ? widthOverride : Width;
            int height = heightOverride > 0 ? heightOverride : Height;
            var gameCamera = camera as Camera;

            if (interactable && hovering && camera is not null)
            {
                _colorOverlay.Parameters["overlayColor"].SetValue(Color.White.ToVector4());
                gameCamera.EnableEffect(_colorOverlay);
                var glowBounds = new Rectangle((int)Bounds.X - 2, (int)Bounds.Y - 2, width + 4, height + 4);
                _spriteBatch.Draw(tileSet, glowBounds, sourceRectangle, Color.White, rotation, Vector2.Zero, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                gameCamera.DisableEffect();
            }

            var rect = new Rectangle((int)(Bounds.X + 0.05), (int)(Bounds.Y + 0.05), width, height);
            Color light = lightColor is not null ? new Color(lightColor.Value) : Color.White;
            _spriteBatch.Draw(tileSet, rect, sourceRectangle, light, rotation, Vector2.Zero, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public void SetDuration(TimeSpan duration)
        {
            
        }

        public void Update()
        {
            hovering = Bounds.Intersects(_mouseHelper.WorldPos.AsRectangle());
        }
    }
}
