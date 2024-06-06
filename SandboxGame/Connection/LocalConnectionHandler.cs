using SandboxGame.Server;
using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    public class LocalConnectionHandler : IConnectionHandler
    {
        private GameServer _server;
        private LocalServerHandler _serverHandler;
        private IServiceProvider services;

        public event IConnectionHandler.HandlePacketEvent HandlePacket;
        public event IConnectionHandler.ConnectionEstablishedEvent ConnectionEstablished;

        public LocalConnectionHandler() 
        {
        }

        public void Connect(string address, int port)
        {
            _serverHandler = new LocalServerHandler(this);
            _server = services.GetService(typeof(GameServer)) as GameServer;
            _server.StartAsync(new CancellationTokenSource().Token);
            ConnectionEstablished?.Invoke();
        }

        public void SendPacket(IPacket packet)
        {
            // Send a packet to the server, hence calling fake receive
            _serverHandler.FakeReceive(packet);
        }

        /// <summary>
        /// This method is not part of the interface, but used for the fake communication between the server and the client.
        /// </summary>
        /// <param name="packet"></param>
        public void ReceivePacket(IPacket packet)
        {
            HandlePacket?.Invoke(packet);
        }
    }
}
