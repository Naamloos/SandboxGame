using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    internal class Camera
    {
        private SpriteBatch _spriteBatch;
        private GameWindow _gameWindow;

        private Vector2 _position = new Vector2(0, 0);
        private float _zoom = 1f;
        private float _rotation = 0f;

        private Vector2 _moveTowards = Vector2.Zero;

        private float distancePerSecond = 1f;

        public Vector2 ScreenCenter
        {
            get => new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
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
            var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            var distanceTraveled = (distancePerSecond / 1000) * frameTime;

            if (Vector2.Distance(_position, _moveTowards) > distanceTraveled)
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

        public void DrawOnCamera(Action draw)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: _translationMatrix);

            draw();

            _spriteBatch.End();
        }
    }
}
