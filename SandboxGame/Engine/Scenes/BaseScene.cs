using Microsoft.Xna.Framework;
using System;

namespace SandboxGame.Engine.Scenes
{
    public abstract class BaseScene : IDisposable
    {
        private bool _contextSet = false;

        public GameContext GameContext 
        { 
            protected get
            {
                if(!_contextSet)
                {
                    throw new InvalidOperationException();
                }

                return _gameContext;
            }
            set
            {
                if (_contextSet)
                {
                    throw new InvalidOperationException();
                }
                _contextSet = true;
                _gameContext = value;
            }
        }
        private GameContext _gameContext;

        public abstract void Initialize();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void Dispose();
    }
}
