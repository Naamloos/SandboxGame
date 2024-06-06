using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Server.EntityManager;
using SandboxGame.Server.GameLogic.Entities;
using SandboxGame.Server.Packets;
using SandboxGame.Server.ServerHandler;

namespace SandboxGame.Server
{
    /// <summary>
    /// Higher level server abstraction. This code should be able to run either remotely or locally, depending on the implementation of IServerHandler.
    /// </summary>
    public class GameServer : IHostedService
    {
        private IServerHandler _serverHandler;

        private ServerEntityManager _entityManager;
        private ILogger<GameServer> _logger;

        private Dictionary<string, CancellationTokenSource> _ghostClients = new Dictionary<string, CancellationTokenSource>();
        private Dictionary<string, RemoteClient> _connectedClients = new Dictionary<string, RemoteClient>();

        // TODO pass server config into GameServer constructor
        public GameServer(IServerHandler serverHandler, ServerEntityManager entityManager, ILogger<GameServer> logger)
        {
            _serverHandler = serverHandler;
            _serverHandler.HandlePacket += handlePacket;
            _serverHandler.HandleConnect += handleConnect;
            _serverHandler.HandleDisconnect += handleDisconnect;

            _entityManager = entityManager;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken ct)
        {
            _serverHandler.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken ct)
        {
            _serverHandler.Stop();
            return Task.CompletedTask;
        }

        const int TICK_RATE = 20; // 20 ticks per second
        private async Task ServerTickLoop(CancellationToken ct)
        {
            var sw = new Stopwatch();
            while (!ct.IsCancellationRequested)
            {
                sw.Start();
                tick();
                sw.Stop();

                var tickDuration = 1000 / TICK_RATE;
                if (sw.ElapsedMilliseconds > tickDuration)
                {
                    _logger.LogWarning($"Server tick took too long: {sw.ElapsedMilliseconds}ms");
                }
                else
                {
                    await Task.Delay((int)(1000 / TICK_RATE - sw.ElapsedMilliseconds), ct);
                }
            }
        }

        private void tick()
        {
            // Tick all services that require ticks
            _entityManager.Tick();
        }

        public void SendPacketToClient(string gameClientIdentifier, IPacket packet)
        {
            var connectionIdentifier = _connectedClients[gameClientIdentifier].ConnectionIdentifier;
            _serverHandler.SendPacketToClient(connectionIdentifier, packet);
        }

        public void SendPacketToAllClients(IPacket packet)
        {
            _serverHandler.SendPacketToAllClients(packet);
        }

        public void SendPacketToAllClientsExcept(string gameClientIdentifier, IPacket packet)
        {
            var connectionIdentifier = _connectedClients[gameClientIdentifier].ConnectionIdentifier;
            _serverHandler.SendPacketToAllClientsExcept(connectionIdentifier, packet);
        }

        private void handleDisconnect(string connectionIdentifier)
        {
            if (!_connectedClients.Any(x => x.Value.ConnectionIdentifier == connectionIdentifier))
            {
                return;
            }

            var client = _connectedClients.First(x => x.Value.ConnectionIdentifier == connectionIdentifier).Value;

            _serverHandler.SendPacketToAllClients(new EntityDespawnPacket()
            {
                EntityId = client.PlayerEntityId // TODO: Get entity ID from clientIdentifier
            });
        }

        private void handleConnect(string connectionIdentifier)
        {
            _logger.LogInformation($"Incoming Connection: {connectionIdentifier}");

            _serverHandler.SendPacketToClient(connectionIdentifier, new ServerInfoPacket());

            var cts = new CancellationTokenSource();
            _ghostClients.Add(connectionIdentifier, cts);

            // if we hear nothing, assume it's either server ping or client is dead
            _ = Task.Run(async () => await Task.Delay(5000, cts.Token).ContinueWith((task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    _serverHandler.SendPacketToClient(connectionIdentifier, new ServerInfoPacket());
                }
            }));
        }

        private void handlePacket(string connectionIdentifier, IPacket packet)
        {
            // Not neccessarily needs a connected client to handle this packet
            if (packet is ClientInfoPacket)
            {
                handleClientInfo(connectionIdentifier, (packet as ClientInfoPacket)!);
            }
            else
            {
                // all other packets must belong to a client, else we ignore them
                if (!_connectedClients.Any(x => x.Value.ConnectionIdentifier == connectionIdentifier))
                {
                    return;
                }

                var client = _connectedClients.First(x => x.Value.ConnectionIdentifier == connectionIdentifier).Value;
                client.HandleIncomingPacket(packet);
            }
        }

        private void handleClientInfo(string connectionIdentifier, ClientInfoPacket clientInfo)
        {
            // if client already exists, ignore
            if (_connectedClients.ContainsKey(connectionIdentifier))
            {
                return;
            }

            if (_ghostClients.ContainsKey(connectionIdentifier))
            {
                _ghostClients[connectionIdentifier].Cancel();
                _ghostClients.Remove(connectionIdentifier);

                var gameClientIdentifier = generateClientIdentifier();
                var newClient = new RemoteClient(gameClientIdentifier, connectionIdentifier, clientInfo, this);
                _connectedClients.Add(gameClientIdentifier, newClient);
                Console.WriteLine($"{clientInfo.Username} has joined!");

                _serverHandler.SendPacketToAllClients(new ChatDataPacket()
                {
                    Message = $"{clientInfo.Username} has joined!"
                });

                var newEntity = _entityManager.SpawnEntity<ServerPlayerEntity>(PointUnit.Zero);
                _entityManager.SetEntityOwner(newEntity.EntityId, gameClientIdentifier);
                newClient.AssignPlayerEntity(newEntity.EntityId);

                // TODO send already existing entities to the new client
            }
        }

        private string generateClientIdentifier()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
