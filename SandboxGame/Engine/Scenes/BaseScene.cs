using Microsoft.Xna.Framework;
using System;

namespace SandboxGame.Engine.Scenes
{
    public abstract class BaseScene : IDisposable
    {
        private bool _contextSet = false;

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void Dispose();
    }
}
