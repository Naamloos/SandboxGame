
using SandboxGame.Engine;
using System;
using System.IO;
using System.Reflection;

namespace SandboxGame
{
    public static class Program
    {
        public static string BASE_SAVE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "savedata");
        public static string WORLDS_PATH = Path.Combine(BASE_SAVE_PATH, "worlds");

        public static void Main(string[] args)
        {
            // ensure savedata directories are available
            if(!Directory.Exists(BASE_SAVE_PATH))
                Directory.CreateDirectory(BASE_SAVE_PATH);

            if(!Directory.Exists(WORLDS_PATH))
                Directory.CreateDirectory(WORLDS_PATH);

            // start running game
            using var game = new Game(new LaunchArgumentParser(args).Parse());
            game.Run();
        }
    }
}