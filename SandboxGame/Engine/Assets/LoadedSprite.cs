﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SandboxGame.Engine.Assets
{
    public class LoadedSprite : IDisposable, ICameraTarget, ILoadedSprite
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<Texture2D> _frames;
        private TimeSpan _duration;
        private int _currentFrame;
        private Effect _colorOverlay;

        private SpriteBatch _spriteBatch;
        private GameTimeHelper _gameTime;
        private MouseHelper _mouseHelper;
        private ICamera _camera;

        public RectangleUnit Bounds { get; private set; }

        public LoadedSprite(int width, int height, TimeSpan duration, Effect colorOverlay, 
            SpriteBatch spriteBatch, GameTimeHelper gameTimeHelper, MouseHelper mouseHelper, ICamera camera, params Texture2D[] frames)
        {
            Width = width;
            Height = height;
            _duration = duration;
            _currentFrame = 0;
            _frames = frames.ToList();
            _colorOverlay = colorOverlay;
            _spriteBatch = spriteBatch;
            _gameTime = gameTimeHelper;
            _mouseHelper = mouseHelper;
            _camera = camera;
        }

        private bool hovering = false;

        public void Update()
        {
            var timer = _gameTime.TotalGameTime.TotalMilliseconds % _duration.TotalMilliseconds;
            var progress = (100 / _duration.TotalMilliseconds) * timer;
            _currentFrame = (int)Math.Abs((_frames.Count * progress) / 100);

            hovering = Bounds.Intersects(_mouseHelper.WorldPos.AsRectangle());
        }

        public void Draw(RectangleUnit destination, bool interactable = false, bool flip = false, uint? lightColor = null, float rotation = 0)
        {
            var gameCamera = _camera as Camera;

            Bounds = destination;

            if(interactable && hovering && _camera is not null)
            {
                _colorOverlay.Parameters["overlayColor"].SetValue(Color.White.ToVector4());
                gameCamera.EnableEffect(_colorOverlay);
                var glowBounds = new Rectangle((int)Bounds.X - 2, (int)Bounds.Y - 2, (int)Bounds.Width + 4, (int)Bounds.Height + 4);
                _spriteBatch.Draw(_frames[_currentFrame], glowBounds, null, Color.White, 0f, Vector2.Zero, flip ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                gameCamera.DisableEffect();
            }

            Color light = lightColor is not null ? new Color(lightColor.Value) : Color.White;

            var rect = new Rectangle((int)Bounds.X, (int)Bounds.Y, (int)Bounds.Width, (int)Bounds.Height);
            _spriteBatch.Draw(_frames[_currentFrame], rect, null, light, rotation, Vector2.Zero, flip? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
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

        public ILoadedSprite Copy()
        {
            return new LoadedSprite(Width, Height, _duration, _colorOverlay, _spriteBatch, _gameTime, _mouseHelper, _camera, _frames.ToArray());
        }
    }
}
