using SandboxGame.Server.Attributes;
using SandboxGame.Server.Packets;
using SandboxGame.Server.Serialization;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.ServerHandler
{
    public class RemoteServerHandler : IServerHandler
    {
        public event IServerHandler.HandlePacketEvent? HandlePacket;
        public event IServerHandler.HandleDisconnectEvent HandleDisconnect;
        public event IServerHandler.HandleConnectEvent HandleConnect;

        private SimpleTcpServer _server;
        private List<string> _clients = new List<string>();
        private Dictionary<long, Type> _packetTypes = new Dictionary<long, Type>();

        private IPacketSerializer _serializer;

        public RemoteServerHandler()
        {
            _server = new SimpleTcpServer(null, 42069); // open on port 42069

            // hook events
            _server.Events.ClientConnected += clientConnected;
            _server.Events.ClientDisconnected += clientDisconnected;
            _server.Events.DataReceived += dataReceived;

            // Register packet types
            var packetTypes = GetType().Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IPacket)) && t != typeof(IPacket));
            foreach (var packetType in packetTypes)
            {
                var packetAttribute = packetType.GetCustomAttribute<PacketAttribute>();
                if (packetAttribute == null)
                    continue;

                if (_packetTypes.ContainsKey(packetAttribute.Id))
                    throw new Exception($"Packet ID {packetAttribute.Id} is already registered.");

                _packetTypes.Add(packetAttribute.Id, packetType);
                Console.WriteLine($"Registered {packetAttribute.Id} ({packetAttribute.Name})");
            }

            _serializer = new ProtoBufPacketSerializer();
        }

        public void Start() => _server.Start();

        public void Stop() => _server.Stop();

        private void clientConnected(object? sender, ConnectionEventArgs e)
        {
            _clients.Add(e.IpPort);
            HandleConnect?.Invoke(e.IpPort);
        }

        private void clientDisconnected(object? sender, ConnectionEventArgs e)
        {
            if (!_clients.Contains(e.IpPort))
                return;

            _clients.Remove(e.IpPort);
            HandleDisconnect?.Invoke(e.IpPort);
        }

        private void dataReceived(object? sender, DataReceivedEventArgs e)
        {
            if (!_clients.Contains(e.IpPort))
                return;

            // interpret first bytes as packet ID long, from ArraySegment<byte>
            var packetId = BitConverter.ToInt64(e.Data.Take(8).ToArray(), 0);
            var packetType = _packetTypes[packetId];
            // get the rest of the data in bytes
            var packetData = e.Data.Skip(8).ToArray();
            // deserialize the packet
            var packet = _serializer.Deserialize(packetType, packetData);

            HandlePacket?.Invoke(e.IpPort, (packet as IPacket)!);
        }

        public void SendPacketToClient(string clientIdentifier, IPacket packet)
        {
            if (_clients.Contains(clientIdentifier))
                sendToClient(clientIdentifier, packetToBytes(packet));
        }

        public void SendPacketToAllClients(IPacket packet)
        {
            var data = packetToBytes(packet);
            foreach (var client in _clients)
            {
                sendToClient(client, data);
            }
        }

        public void SendPacketToAllClientsExcept(string clientIdentifier, IPacket packet)
        {
            var data = packetToBytes(packet);
            foreach (var client in _clients)
            {
                if (client == clientIdentifier)
                    continue;

                sendToClient(client, data);
            }
        }

        private byte[] packetToBytes(IPacket packet)
        {
            var packetId = _packetTypes.Where(x => x.Value == packet.GetType()).FirstOrDefault().Key;
            using var ms = new MemoryStream();
            // write packet ID as bytes
            ms.Write(BitConverter.GetBytes(packetId));
            // serialize packet to bytes
            ms.Write(_serializer.Serialize(packet));

            return ms.ToArray();
        }

        private void sendToClient(string clientIdentifier, byte[] data)
        {
            _server.Send(clientIdentifier, data);
        }
    }
}
