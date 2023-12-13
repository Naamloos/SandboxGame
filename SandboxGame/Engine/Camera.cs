using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public class Camera
    {
        private SpriteBatch _spriteBatch;
        private GameWindow _gameWindow;

        private Vector2 _position = new Vector2(0, 0);
        private float _zoom = 1f;
        private float _rotation = 0f;

        private Vector2 _moveTowards = Vector2.Zero;
        private Vector2 _startingPosition;
        private float distancePerSecond = 1f;

        private Sprite following = null;
        private bool smoothFollow = false;

        public Vector2 ScreenCenter
        {
            get => new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
        }

        public bool IsFollowing
        {
            get => following != null;
        }

        public Sprite Target
        {
            get => following;
        }

        private Matrix _translationMatrix
        {
            get => Matrix.CreateTranslation(-(int)_position.X, -(int)_position.Y, 0)
                * Matrix.CreateRotationZ(_rotation)
                * Matrix.CreateScale(new Vector3(_zoom, _zoom, 1))
                * Matrix.CreateTranslation(new Vector3(ScreenCenter, 0));
        }

        public Camera(SpriteBatch spriteBatch, GameWindow window)
        {
            _spriteBatch = spriteBatch;
            _gameWindow = window;
            _position = new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
            _moveTowards = _position;
        }

        public void Reset()
        {
            _moveTowards = new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
        }

        public void SetSpeed(float traveledPerSecond)
        {
            distancePerSecond = traveledPerSecond;
        }

        public void SetZoom(float zoom)
        {
            _zoom = zoom;
        }

        public void SetTarget(Vector2 moveTowards)
        {
            _moveTowards = moveTowards;
        }

        public bool IsMoving()
        {
            return _moveTowards != _position;
        }

        public void Update(GameTime gameTime)
        {
            if (following is not null)
            {
                if (smoothFollow)
                {
                    _moveTowards = following.Bounds.Center.ToVector2();
                }
                else
                {
                    _position = following.Bounds.Center.ToVector2();
                    return;
                }
            }

            var distance = Vector2.Distance(_position, _moveTowards);
            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = ((distancePerSecond / 1000) * frameTime) * (distance / 100);

            if (distance > distanceTraveled)
            {
                var direction = _moveTowards - _position;
                direction.Normalize();

                _position += direction * distanceTraveled;
            }
            else
            {
                _startingPosition = _moveTowards;
                _position = _moveTowards;
            }

            DebugHelper.SetDebugValues("CAMERA", $"x: {_position.X.ToString().PadRight(15)} y: {_position.Y.ToString().PadRight(15)}");
        }

        public Vector2 WorldToScreen(Vector2 worldCoords)
        {
            return Vector2.Transform(worldCoords, _translationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 screenCoords)
        {
            return Vector2.Transform(screenCoords, Matrix.Invert(_translationMatrix));
        }

        public void DrawOnCamera(Action draw)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _translationMatrix);

            draw();

            _spriteBatch.End();
        }

        public void EnableEffect(Effect effect)
        {
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, effect: effect, transformMatrix: _translationMatrix);
        }

        public void DisableEffect()
        {
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _translationMatrix);
        }

        public void Follow(Sprite sprite, bool smooth = true)
        {
            following = sprite;
            smoothFollow = smooth;
        }

        public void StopFollowing(bool resetToLastPosition = true)
        {
            following = null;
            if(!resetToLastPosition)
            {
                _moveTowards = _position;
            }
        }
    }
}
