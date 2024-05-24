
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.DependencyInjection;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using SandboxGame.Engine.Scenes;
using SandboxGame.Engine.Storage;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame
{
    public static class Program
    {
        public static string BASE_SAVE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "savedata");
        public static string WORLDS_PATH = Path.Combine(BASE_SAVE_PATH, "worlds");
        private static string[] ARGS;

        public static Game Game { get; private set; } // HACK

        public static void Main(string[] args)
        {
            ARGS = args;

            // ensure savedata directories are available
            if(!Directory.Exists(BASE_SAVE_PATH))
                Directory.CreateDirectory(BASE_SAVE_PATH);

            if(!Directory.Exists(WORLDS_PATH))
                Directory.CreateDirectory(WORLDS_PATH);

            Game = new Game();

            // Assets can only be loaded in loadContent, 
            // I mean- you can load some stuff later but just to be safe
            Game.OnLoadContent += loadContent;

            Game.Run();
        }

        private static void loadContent(Game game)
        {
            Task.Run(() => CreateHostBuilder(ARGS, game).Build().Run());
        }

        public static IHostBuilder CreateHostBuilder(string[] args, Game game)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<GameHostService>();
                    services.AddSingleton<GraphicsDevice>(x => game.GraphicsDevice);
                    services.AddSingleton<SpriteBatch>(x => game.SpriteBatch);
                    services.AddSingleton<IStorageSupplier>(new FileStorageSupplier());
                    services.AddSingleton<SceneManager>();
                    services.AddSingleton<AssetManager>();
                    services.AddSingleton<Camera>();
                    services.AddSingleton<GameWindow>(x => game.Window);
                    services.AddSingleton<MouseHelper>();
                    services.AddSingleton<InputHelper>();
                    services.AddSingleton<KeybindManager>();
                    services.AddSingleton<LaunchArgs>(new LaunchArgumentParser(args).Parse());
                    services.AddSingleton<ContentManager>(x => game.Content);
                    services.AddSingleton<EntityManager>();
                    services.AddSingleton<GraphicsDeviceManager>(x => game.GraphicsDeviceManager);
                });
        }
    }
}