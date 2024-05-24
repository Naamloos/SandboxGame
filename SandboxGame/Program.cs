
using System.IO;
using System.Reflection;

namespace SandboxGame
{
    public static class Program
    {
        public static string BASE_SAVE_PATH = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "savedata");
        public static string WORLDS_PATH = Path.Combine(BASE_SAVE_PATH, "worlds");
        public static string PROFILES_PATH = Path.Combine(BASE_SAVE_PATH, "profiles");

        public static void Main(string[] args)
        {
            // ensure savedata directories are available
            if(!Directory.Exists(BASE_SAVE_PATH))
                Directory.CreateDirectory(BASE_SAVE_PATH);

            if(!Directory.Exists(WORLDS_PATH))
                Directory.CreateDirectory(WORLDS_PATH);

            if(!Directory.Exists(PROFILES_PATH))
                Directory.CreateDirectory(PROFILES_PATH);

            // start running game
            using var game = new Game(args);
            game.Run();
        }
    }
}