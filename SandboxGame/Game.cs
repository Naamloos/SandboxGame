using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Scenes;
using SandboxGame.Engine.Storage;
using SandboxGame.Engine.Storage.Serialization;
using SandboxGame.Scenes;
using System;
using System.Security.Principal;

namespace SandboxGame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public delegate void OnUpdateDelegate(GameTime gameTime);
        public delegate void OnDrawDelegate(GameTime gameTime);
        public delegate void OnLoadContentDelegate(Game game);

        public event OnUpdateDelegate OnUpdate;
        public event OnDrawDelegate OnDraw;
        public event OnLoadContentDelegate OnLoadContent;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        public Game()
        {
            this.GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            IsFixedTimeStep = true; // set to false to unlock frame rate.
        }

        protected override void Initialize()
        {
            Window.Title = "SandBoxGame Pre-Alpha Indev Whatever";
            this.SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            OnLoadContent?.Invoke(this);
        }

        protected override void Update(GameTime gameTime)
        {
            OnUpdate?.Invoke(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            OnDraw?.Invoke(gameTime);
        }
    }
}
