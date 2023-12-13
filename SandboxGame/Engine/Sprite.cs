﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public class Sprite : IDisposable, ICameraTarget
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<Texture2D> _frames;
        private TimeSpan _duration;
        private int _currentFrame;
        private Effect _colorOverlay;

        public Rectangle Bounds { get; private set; }

        public Sprite(int width, int height, TimeSpan duration, Effect colorOverlay, params Texture2D[] frames)
        {
            Width = width;
            Height = height;
            _duration = duration;
            _currentFrame = 0;
            _frames = frames.ToList();
            _colorOverlay = colorOverlay;
        }

        public void Update(GameTime gameTime)
        {
            var timer = gameTime.TotalGameTime.TotalMilliseconds % _duration.TotalMilliseconds;
            var progress = (100 / _duration.TotalMilliseconds) * timer;
            _currentFrame = (int)Math.Abs((_frames.Count * progress) / 100);
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y, bool bloom = false, bool flip = false,
            Camera camera = null, Color? lightColor = null, int widthOverride = -1, int heightOverride = -1)
        {
            int width = widthOverride > 0 ? widthOverride : Width;
            int height = heightOverride > 0 ? heightOverride : Height;

            Bounds = new Rectangle(x, y, width, height);

            if(bloom && camera is not null)
            {
                _colorOverlay.Parameters["overlayColor"].SetValue(Color.White.ToVector4());
                camera.EnableEffect(_colorOverlay);
                var glowBounds = new Rectangle(x - 2, y - 2, width + 4, height + 4);
                spriteBatch.Draw(_frames[_currentFrame], glowBounds, null, Color.White, 0f, Vector2.Zero, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                camera.DisableEffect();
            }

            spriteBatch.Draw(_frames[_currentFrame], Bounds, null, lightColor ?? Color.White, 0f, Vector2.Zero, flip? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
        }

        public void SetDuration(TimeSpan duration)
        {
            _duration = duration;
        }

        public void Dispose()
        {
            // Getting rid of loose references before disposing
            _frames.Clear();
        }

        public Sprite Copy()
        {
            return new Sprite(Width, Height, _duration, _colorOverlay ,_frames.ToArray());
        }
    }
}