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

        private Vector2 _center
        {
            get => new Vector2(_gameWindow.ClientBounds.Width * 0.5f, _gameWindow.ClientBounds.Height * 0.5f);
        }

        private Matrix _translationMatrix
        {
            get => Matrix.CreateTranslation(-(int)_position.X, -(int)_position.Y, 0)
                * Matrix.CreateRotationZ(_rotation)
                * Matrix.CreateScale(new Vector3(_zoom, _zoom, 1))
                * Matrix.CreateTranslation(new Vector3(_center, 0));
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

        public void MoveTowards(Vector2 moveTowards)
        {
            _moveTowards = moveTowards;
        }

        public bool IsMoving()
        {
            return _moveTowards != _position;
        }

        public void Update(GameTime gameTime)
        {
            if (Vector2.Distance(_position, _moveTowards) > 5)
            {
                var frameTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                var direction = _moveTowards - _position;
                direction.Normalize();

                var distanceTraveled = (distancePerSecond / 1000) * frameTime;
                _position += direction * distanceTraveled;
            }
        }

        public void DrawOnCamera(Action draw)
        {
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: _translationMatrix);

            draw();

            _spriteBatch.End();
        }
    }
}
