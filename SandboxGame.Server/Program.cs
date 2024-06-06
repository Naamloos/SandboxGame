using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SandboxGame.Api.Entity;
using SandboxGame.Server.EntityManager;
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
                    ServerServices.Register(services);
                })
                .Build();
        }
    }

    public static class ServerServices
    {
        /// <summary>
        /// Can be reused by the client to register server dependencies if in single player mode.
        /// </summary>
        /// <param name="services"></param>
        public static void Register(IServiceCollection services)
        {
            services.AddHostedService<GameServer>();
            services.AddSingleton<IServerEntityManager, ServerEntityManager>();
            services.AddSingleton<ServerEntityManager>(x => x.GetService<IServerEntityManager>() as ServerEntityManager);
        }
    }
}
