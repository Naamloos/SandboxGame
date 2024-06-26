﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SandboxGame.Engine.Cameras
{
    public class Camera : ICamera
    {
        private SpriteBatch _spriteBatch;
        private GameWindow _gameWindow;
        private MouseHelper _mouseHelper;

        private PointUnit _position = PointUnit.Zero;
        private float _zoom = 1.5f;
        private float _targetZoom = 1.5f; // initial value same as camera initial value
        private bool _focusZoom = false;
        private float _rotation = 0f;

        private PointUnit _moveTowards = PointUnit.Zero;
        private float distancePerSecond = 1f;

        private ICameraTarget defaultFollow = null;
        private ICameraTarget following = null;
        private bool smoothFollow = true;

        private const float MAX_ZOOM = 6f;
        private const float MIN_ZOOM = 0.8f;

        public RectangleUnit WorldView
        {
            get
            {
                var xy = ScreenToWorld(new PointUnit(0, 0));
                var wh = ScreenToWorld(new PointUnit(_gameWindow.ClientBounds.Width, _gameWindow.ClientBounds.Height));
                return new RectangleUnit(xy.X, xy.Y, wh.X, wh.Y);
            }
        }

        public RectangleUnit ScreenView
        {
            get => new RectangleUnit(0, 0, _gameWindow.ClientBounds.Width, _gameWindow.ClientBounds.Height);
        }

        public PointUnit ScreenCenter
        {
            get => new PointUnit(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
        }

        public bool IsFollowing
        {
            get => following != null;
        }

        public ICameraTarget Target
        {
            get => following;
        }

        public float Zoom
        {
            get => _zoom;
        }

        private Matrix _translationMatrix
        {
            get => Matrix.CreateTranslation(-_position.X, -_position.Y, 0)
                * Matrix.CreateRotationZ(_rotation)
                * Matrix.CreateScale(new Vector3(_zoom, _zoom, 1))
                * Matrix.CreateTranslation(new Vector3(ScreenCenter.X, ScreenCenter.Y, 0));
        }

        public Camera(SpriteBatch spriteBatch, GameWindow window, MouseHelper mouseHelper)
        {
            _spriteBatch = spriteBatch;
            _gameWindow = window;
            _position = new PointUnit(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
            _moveTowards = _position;
            _mouseHelper = mouseHelper;
        }

        public void Reset()
        {
            _moveTowards = new PointUnit(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
        }

        public void SetSpeed(float traveledPerSecond)
        {
            distancePerSecond = traveledPerSecond;
        }

        public void SetZoom(float zoom)
        {
            _focusZoom = false;
            _zoom = zoom;
        }

        public void FocusZoom(float zoom)
        {
            _focusZoom = true;
            _targetZoom = zoom;
        }

        public bool IsMoving()
        {
            return _moveTowards != _position;
        }

        public void Update(GameTime gameTime)
        {
            //if (_mouseHelper != null && !_focusZoom)
            //{
            //    if (_mouseHelper.ScrollUp && _zoom < MAX_ZOOM)
            //    {
            //        _zoom += 0.2f;
            //    }
            //    if (_mouseHelper.ScrollDown && _zoom > MIN_ZOOM)
            //    {
            //        _zoom -= 0.2f;
            //    }
            //}

            // smooth zoom and _zoom within 0.2 of _targetZoom
            if (_focusZoom && Math.Abs(_zoom - _targetZoom) > 0.2f)
            {
                if(_zoom < _targetZoom + 0.1f)
                {
                    _zoom += 0.1f;
                }
                else if(_zoom > _targetZoom)
                {
                    _zoom -= 0.1f;
                }
            }

            // clamp zoom within MAX_ZOOM and MIN_ZOOM values
            _zoom = MathHelper.Clamp(_zoom, MIN_ZOOM, MAX_ZOOM);

            // If we're following a sprite, we update the _moveTowards target.
            if (following is not null)
            {
                if (smoothFollow)
                {
                    _moveTowards = following.Bounds.Center;
                }
                else
                {
                    _position = following.Bounds.Center;
                    return;
                }
            }

            var distance = PointUnit.Distance(_position, _moveTowards);
            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            var distanceTraveled = (distancePerSecond / 1000 * frameTime) * (distance / 1000 * frameTime);

            if (distance > distanceTraveled)
            {
                var direction = _moveTowards - _position;
                direction.Normalize();

                _position += direction * distanceTraveled;
            }
            else
            {
                _position = _moveTowards;
            }

            var screenToWorld = ScreenToWorld(new PointUnit((int)_mouseHelper.ScreenPos.X, (int)_mouseHelper.ScreenPos.Y));
            _mouseHelper.WorldPos = new PointUnit(screenToWorld.X, screenToWorld.Y); // TODO implement translation matrixes for my own units

            DebugHelper.SetDebugValues("CAMERA", $"x: {_position.X.ToString().PadRight(15)} y: {_position.Y.ToString().PadRight(15)}");
        }

        private Vector2 WorldToScreen(Vector2 worldCoords)
        {
            return Vector2.Transform(worldCoords, _translationMatrix);
        }

        private Vector2 ScreenToWorld(Vector2 screenCoords)
        {
            return Vector2.Transform(screenCoords, Matrix.Invert(_translationMatrix));
        }

        public PointUnit WorldToScreen(PointUnit worldCoords)
        {
            var screenCoords = WorldToScreen(new Vector2(worldCoords.X, worldCoords.Y));
            return new PointUnit(screenCoords.X, screenCoords.Y);
        }

        public PointUnit ScreenToWorld(PointUnit screenCoords)
        {
            var worldCoords = ScreenToWorld(new Vector2(screenCoords.X, screenCoords.Y));
            return new PointUnit(worldCoords.X, worldCoords.Y);
        }

        public void DrawOnCamera(Action draw)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _translationMatrix);

            draw();

            _spriteBatch.End();
        }

        private List<Action> _uiDraws = new List<Action>();
        public void DrawToUI(Action draw)
        {
            _uiDraws.Add(draw);
        }

        public void FlushUIDraw()
        {
            for(int i = 0; i < _uiDraws.Count; i++)
            {
                _uiDraws[i]();
            }

            _uiDraws.Clear();
        }

        public void EnableEffect(Effect effect)
        {
            // Restarts the spritebatch with an effect applied
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, effect: effect, transformMatrix: _translationMatrix);
        }

        public void DisableEffect()
        {
            // Restarts the spritebatch with no effect applied
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _translationMatrix);
        }

        public void SetDefaultFollow(ICameraTarget target = null)
        {
            this.defaultFollow = target;
            if (this.following is null)
                following = target;
        }

        public void Follow(ICameraTarget target, bool smooth = true)
        {
            // Setting our new follow target
            following = target;
            smoothFollow = smooth;
        }

        public void StopFollowing(bool resetToLastPosition = true)
        {
            // Stops following a target
            following = defaultFollow;
            if(!resetToLastPosition)
            {
                _moveTowards = _position;
            }
        }
    }
}
