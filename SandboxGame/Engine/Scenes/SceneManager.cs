using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Scenes
{
    public class SceneManager
    {
        private SemaphoreSlim _updateLock;
        private SemaphoreSlim _drawLock;
        private BaseScene? currentScene = null;

        private IServiceProvider _serviceProvider;

        // TODO INJECT A COUPLE OF THINGS IN SCENES BY DEFAULT? SPRITEBATCH, SCENEMANAGER, CAMERA, INPUT/MOUSE HELPERS
        public SceneManager(IServiceProvider services) 
        {
            _serviceProvider = services;
            _updateLock = new SemaphoreSlim(1);
            _drawLock = new SemaphoreSlim(1);
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

                var args = typeof(T).GetConstructors().First().GetParameters().Select(x => _serviceProvider.GetService(x.ParameterType)).ToArray();
                currentScene = (T)Activator.CreateInstance(typeof(T), args);
                currentScene.Initialize();
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
    }
}
