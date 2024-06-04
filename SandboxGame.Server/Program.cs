using SandboxGame.Server.ServerHandler;

namespace SandboxGame.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var remoteServerHandler = new RemoteServerHandler();

            var server = new GameServer(remoteServerHandler);

            server.Start();
            Console.WriteLine("Server Started");
            while(true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
