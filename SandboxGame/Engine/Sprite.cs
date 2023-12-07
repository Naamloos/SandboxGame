using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public class Sprite : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private List<Texture2D> _frames;
        private TimeSpan _duration;
        private int _currentFrame;

        public Rectangle Bounds { get; private set; }

        private Vector2 _lastPosition = Vector2.Zero;

        private RenderTarget2D _renderTarget;

        public Sprite(int width, int height, TimeSpan duration, params Texture2D[] frames)
        {
            Width = width;
            Height = height;
            _duration = duration;
            _currentFrame = 0;
            _frames = frames.ToList();
        }

        public void Update(GameTime gameTime)
        {
            var timer = gameTime.TotalGameTime.TotalMilliseconds % _duration.TotalMilliseconds;
            var progress = (100 / _duration.TotalMilliseconds) * timer;
            _currentFrame = (int)Math.Abs((_frames.Count * progress) / 100);
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y, bool bloom = false,
            Color? lightColor = null, int widthOverride = -1, int heightOverride = -1)
        {
            int width = widthOverride > 0 ? widthOverride : Width;
            int height = heightOverride > 0 ? heightOverride : Height;

            Bounds = new Rectangle(x, y, width, height);
            spriteBatch.Draw(_frames[_currentFrame], Bounds, lightColor ?? Color.White);
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
            return new Sprite(Width, Height, _duration, _frames.ToArray());
        }
    }
}
