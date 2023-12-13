using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Cameras
{
    public class Camera
    {
        private SpriteBatch _spriteBatch;
        private GameWindow _gameWindow;
        private MouseHelper _mouseHelper;

        private Vector2 _position = new Vector2(0, 0);
        private float _zoom = 2.4f;
        private float _rotation = 0f;

        private Vector2 _moveTowards = Vector2.Zero;
        private float distancePerSecond = 1f;

        private ICameraTarget defaultFollow = null;
        private ICameraTarget following = null;
        private bool smoothFollow = true;

        private const float MAX_ZOOM = 6f;
        private const float MIN_ZOOM = 0.5f;

        public Vector2 ScreenCenter
        {
            get => new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
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

        public bool IsMoving()
        {
            return _moveTowards != _position;
        }

        public void SetMouseHelper(MouseHelper mouseHelper)
        {
            _mouseHelper = mouseHelper;
        }

        public void Update(GameTime gameTime)
        {
            if (_mouseHelper != null)
            {
                if (_mouseHelper.ScrollUp && _zoom < MAX_ZOOM)
                {
                    _zoom += 0.15f;
                }
                if (_mouseHelper.ScrollDown && _zoom > MIN_ZOOM)
                {
                    _zoom -= 0.15f;
                }
            }

            SetZoom(_zoom);

            // If we're following a sprite, we update the _moveTowards target.
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

        private List<Action> _uiDraws = new List<Action>();
        public void DrawToUI(Action draw)
        {
            _uiDraws.Add(draw);
        }

        public void FlushUIDraw()
        {
            foreach(var action in _uiDraws)
            {
                action();
            }

            _uiDraws.Clear();
        }

        public void EnableEffect(Effect effect)
        {
            // Restarts the spritebatch with an effect applied
            _spriteBatch.End();
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, effect: effect, transformMatrix: _translationMatrix);
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
