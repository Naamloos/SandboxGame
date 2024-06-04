using SandboxGame.Server.Packets;
using SandboxGame.Server.ServerHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    /// <summary>
    /// Fake server handler that is used to communicate with a built-in server.
    /// </summary>
    public class LocalServerHandler : IServerHandler
    {
        public event IServerHandler.HandlePacketEvent HandlePacket;
        public event IServerHandler.HandleDisconnectEvent HandleDisconnect;
        public event IServerHandler.HandleConnectEvent HandleConnect;

        private LocalConnectionHandler _localConnection;

        public LocalServerHandler(LocalConnectionHandler localConnection) 
        {
            _localConnection = localConnection;
        }

        public void FakeReceive(IPacket packet)
        {
            HandlePacket?.Invoke("local", packet);
        }

        public void SendPacketToAllClients(IPacket packet)
        {
            // only sends to the local client
            _localConnection.ReceivePacket(packet);
        }

        public void SendPacketToAllClientsExcept(string clientIdentifier, IPacket packet)
        {
            if(clientIdentifier == "local")
            {
                return;
            }
            // if the client identifier is not us (somehow), we'll still receive it.
            _localConnection.ReceivePacket(packet);
        }

        public void SendPacketToClient(string clientIdentifier, IPacket packet)
        {
            // no identifier because this is a local connection
            _localConnection.ReceivePacket(packet);
        }

        public void Start()
        {
            HandleConnect("local");
        }

        public void Stop()
        {
            HandleDisconnect("local");
        }
    }
}
