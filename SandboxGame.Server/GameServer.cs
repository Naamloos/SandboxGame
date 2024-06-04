using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxGame.Server.Packets;
using SandboxGame.Server.ServerHandler;

namespace SandboxGame.Server
{
    /// <summary>
    /// Higher level server abstraction. This code should be able to run either remotely or locally, depending on the implementation of IServerHandler.
    /// </summary>
    public class GameServer
    {
        private IServerHandler _serverHandler;

        private Dictionary<string, CancellationTokenSource> _ghostClients = new Dictionary<string, CancellationTokenSource>();
        private Dictionary<string, RemoteClient> _connectedClients = new Dictionary<string, RemoteClient>();

        // TODO pass server config into GameServer constructor
        public GameServer(IServerHandler serverHandler)
        {
            _serverHandler = serverHandler;
            _serverHandler.HandlePacket += handlePacket;
            _serverHandler.HandleConnect += handleConnect;
            _serverHandler.HandleDisconnect += handleDisconnect;
        }

        public void Start()
        {
           _serverHandler.Start();
        }

        public void Stop()
        {
           _serverHandler.Stop();
        }

        public void SendPacketToClient(string clientIdentifier, IPacket packet)
        {
            _serverHandler.SendPacketToClient(clientIdentifier, packet);
        }

        public void SendPacketToAllClients(IPacket packet)
        {
            _serverHandler.SendPacketToAllClients(packet);
        }

        public void SendPacketToAllClientsExcept(string clientIdentifier, IPacket packet)
        {
            _serverHandler.SendPacketToAllClientsExcept(clientIdentifier, packet);
        }

        private void handleDisconnect(string clientIdentifier)
        {
            _serverHandler.SendPacketToAllClients(new EntityDespawnPacket()
            {
                EntityId = "" // TODO: Get entity ID from clientIdentifier
            });
        }

        private void handleConnect(string clientIdentifier)
        {
            Console.WriteLine($"Incoming Connection: {clientIdentifier}");

            _serverHandler.SendPacketToClient(clientIdentifier, new ServerInfoPacket());

            var cts = new CancellationTokenSource();
            _ghostClients.Add(clientIdentifier, cts);

            // if we hear nothing, assume it's either server ping or client is dead
            _ = Task.Run(async () => await Task.Delay(5000, cts.Token).ContinueWith((task) =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    _serverHandler.SendPacketToClient(clientIdentifier, new ServerInfoPacket());
                }
            }));
        }

        private void handlePacket(string clientIdentifier, IPacket packet)
        {
            // Not neccessarily needs a connected client to handle this packet
            if (packet is ClientInfoPacket)
            {
                handleClientInfo(clientIdentifier, (packet as ClientInfoPacket)!);
            }
            else
            {
                // all other packets must belong to a client, else we ignore them
                if (!_connectedClients.ContainsKey(clientIdentifier))
                {
                    return;
                }

                _connectedClients[clientIdentifier].HandleIncomingPacket(packet);
            }
        }

        private void handleClientInfo(string clientIdentifier, ClientInfoPacket clientInfo)
        {
            // if client already exists, ignore
            if (_connectedClients.ContainsKey(clientIdentifier))
            {
                return;
            }

            if (_ghostClients.ContainsKey(clientIdentifier))
            {
                _ghostClients[clientIdentifier].Cancel();
                _ghostClients.Remove(clientIdentifier);
                _connectedClients.Add(clientIdentifier, new RemoteClient(clientIdentifier, clientInfo, this));
                Console.WriteLine($"{clientInfo.Username} has joined!");
                _serverHandler.SendPacketToAllClients(new ChatDataPacket()
                {
                    Message = $"{clientInfo.Username} has joined!"
                });
            }
        }
    }
}
