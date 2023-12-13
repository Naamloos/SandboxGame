using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Scenes
{
    public class SceneManager
    {
        private GameContext _gameContext;

        private SemaphoreSlim _updateLock;
        private SemaphoreSlim _drawLock;
        private BaseScene? currentScene = null;

        public SceneManager(GameContext gameContext) 
        {
            _updateLock = new SemaphoreSlim(1);
            _drawLock = new SemaphoreSlim(1);

            _gameContext = gameContext;
        }

        public void Switch<T>() where T : BaseScene
        {
            // This should ensure we don't continue drawing and updating when we switch
            // scenes. This to prevent weird UB with helper classes.
            _ = Task.Run(() =>
            {
                _updateLock.Wait();
                _drawLock.Wait();

                if (currentScene != null)
                {
                    currentScene.Dispose();
                }

                currentScene = Activator.CreateInstance<T>();
                currentScene.Initialize(_gameContext);
                _updateLock.Release();
                _drawLock.Release();
            });
        }

        public void Update(GameTime gameTime)
        {
            if(currentScene == null) 
            {
                return;
            }

            _updateLock.Wait();
            currentScene.Update(gameTime);
            _updateLock.Release();
        }

        public void Draw(GameTime gameTime)
        {
            if (currentScene == null)
            {
                return;
            }

            _drawLock.Wait();
            currentScene.Draw(gameTime);
            _drawLock.Release();
        }

        public void DrawUI(GameTime gameTime)
        {
            if (currentScene == null)
            {
                return;
            }

            _drawLock.Wait();
            currentScene.DrawUI(gameTime);
            _drawLock.Release();
        }
    }
}
