using SandboxGame.Server;
using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    public class LocalConnectionHandler : IConnectionHandler
    {
        private GameServer _server;
        private LocalServerHandler _serverHandler;

        public event IConnectionHandler.HandlePacketEvent HandlePacket;
        public event IConnectionHandler.ConnectionEstablishedEvent ConnectionEstablished;

        public LocalConnectionHandler() 
        {
            _serverHandler = new LocalServerHandler(this);
            _server = new GameServer(_serverHandler);
        }

        public void Connect(string address, int port)
        {
            _server.Start();
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
