using SandboxGame.Server;
using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    public interface IConnectionHandler
    {
        public delegate void HandlePacketEvent(IPacket packet);
        public event HandlePacketEvent HandlePacket;

        public delegate void ConnectionEstablishedEvent();
        public event ConnectionEstablishedEvent ConnectionEstablished;

        public void Connect(string address, int port);

        public void SendPacket(IPacket packet);
    }
}
