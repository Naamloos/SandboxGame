
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Camera;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Input;
using SandboxGame.Connection;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.DependencyInjection;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Modding;
using SandboxGame.Engine.Scenes;
using SandboxGame.Engine.Storage;
using System;
using System.Threading.Tasks;

namespace SandboxGame
{
    public static class Program
    {
        private static string[] ARGS;

        public static Game Game { get; private set; } // HACK

        public static void Main(string[] args)
        {
            ARGS = args;

            Game = new Game();

            // Assets can only be loaded in loadContent, 
            // I mean- you can load some stuff later but just to be safe
            Game.OnLoadContent += loadContent;

            Game.Run();
        }

        private static void loadContent(Game game)
        {
            Task.Run(() => CreateHostBuilder(ARGS, game).Build().Run())
                .ContinueWith(t => { 
                    if(t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception);
                        throw t.Exception;
                    }
                });
        }

        public static IHostBuilder CreateHostBuilder(string[] args, Game game)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<GameHostService>();

                    /* Base services */
                    services.AddSingleton<LaunchArgs>(new LaunchArgumentParser(args).Parse());

                    /* Services from MonoGame */
                    services.AddSingleton<GraphicsDevice>(x => game.GraphicsDevice);
                    services.AddSingleton<SpriteBatch>(x => game.SpriteBatch);
                    services.AddSingleton<ContentManager>(x => game.Content);
                    services.AddSingleton<GameWindow>(x => game.Window); 
                    services.AddSingleton<GraphicsDeviceManager>(x => game.GraphicsDeviceManager);

                    /* Game-specific services */
                    services.AddSingleton<IStorageSupplier>(new FileStorageSupplier());
                    services.AddSingleton<SceneManager>();
                    services.AddSingleton<IConnectionHandler, RemoteConnectionHandler>();
                    services.AddSingleton<ConnectionManager>();

                    /* Services also available to Mods */
                    services.AddSingleton<IAssetManager, AssetManager>();
                    services.AddSingleton(x => x.GetService<IAssetManager>() as AssetManager);

                    services.AddSingleton<IEntityManager, EntityManager>();
                    services.AddSingleton(x => x.GetService<IEntityManager>() as EntityManager);

                    services.AddSingleton<ModManager>();

                    services.AddSingleton<IGameTimeHelper, GameTimeHelper>();
                    services.AddSingleton(x => x.GetService<IGameTimeHelper>() as GameTimeHelper);

                    services.AddSingleton<ICamera, Camera>();
                    services.AddSingleton(x => x.GetService<ICamera>() as Camera);

                    services.AddSingleton<IKeybindManager, KeybindManager>();
                    services.AddSingleton(x => x.GetService<IKeybindManager>() as KeybindManager);

                    services.AddSingleton<IInputHelper, InputHelper>();
                    services.AddSingleton(x => x.GetService<IInputHelper>() as InputHelper);

                    services.AddSingleton<IMouseHelper, MouseHelper>();
                    services.AddSingleton(x => x.GetService<IMouseHelper>() as MouseHelper);

                    services.AddLogging();
                });
        }
    }
}