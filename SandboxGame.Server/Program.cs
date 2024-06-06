using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SandboxGame.Server.ServerHandler;

namespace SandboxGame.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IServerHandler, RemoteServerHandler>();
                    RegisterServerDependencies(services);
                })
                .Build();
        }

        /// <summary>
        /// Can be reused by the client to register server dependencies if in single player mode.
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServerDependencies(IServiceCollection services)
        {
            services.AddHostedService<GameServer>();
        }
    }
}
