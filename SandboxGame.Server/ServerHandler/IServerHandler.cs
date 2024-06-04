using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.ServerHandler
{
    /// <summary>
    /// Helper interface so the server implementation can be swapped out for a local variant.
    /// </summary>
    public interface IServerHandler
    {
        public void Start();

        public void Stop();

        public delegate void HandlePacketEvent(string clientIdentifier, IPacket packet);
        public event HandlePacketEvent HandlePacket;

        public delegate void HandleDisconnectEvent(string clientIdentifier);
        public event HandleDisconnectEvent HandleDisconnect;

        public delegate void HandleConnectEvent(string clientIdentifier);
        public event HandleConnectEvent HandleConnect;

        public void SendPacketToClient(string clientIdentifier, IPacket packet);

        public void SendPacketToAllClients(IPacket packet);

        public void SendPacketToAllClientsExcept(string clientIdentifier, IPacket packet);
    }
}
