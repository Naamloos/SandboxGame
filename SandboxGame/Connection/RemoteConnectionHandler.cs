using Microsoft.Extensions.Logging;
using SandboxGame.Server.Attributes;
using SandboxGame.Server.Packets;
using SandboxGame.Server.Serialization;
using SuperSimpleTcp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    public class RemoteConnectionHandler : IConnectionHandler
    {
        public event IConnectionHandler.HandlePacketEvent HandlePacket;
        public event IConnectionHandler.ConnectionEstablishedEvent ConnectionEstablished;

        private SimpleTcpClient _client;
        private IPacketSerializer _serializer;

        private Dictionary<long, Type> _packetTypes = new Dictionary<long, Type>();

        public RemoteConnectionHandler(ILogger<RemoteConnectionHandler> logger)
        {
            _serializer = new ProtoBufPacketSerializer();

            // Register packet types
            var packetTypes = typeof(IPacket).Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IPacket)) && t != typeof(IPacket));
            foreach (var packetType in packetTypes)
            {
                var packetAttribute = packetType.GetCustomAttribute<PacketAttribute>();
                if (packetAttribute == null)
                    continue;

                _packetTypes.Add(packetAttribute.Id, packetType);
                logger.LogInformation($"Registered packet {packetAttribute.Id}: {packetAttribute.Name}");
            }
        }

        public void Connect(string address, int port)
        {
            _client = new SimpleTcpClient(address, port);
            _client.Events.DataReceived += onDataReceived;
            _client.Events.Connected += connectionEstablished;
            _client.Connect();
        }

        private void connectionEstablished(object sender, ConnectionEventArgs e)
        {
            ConnectionEstablished?.Invoke();
        }

        private void onDataReceived(object sender, DataReceivedEventArgs e)
        {
            // interpret first bytes as packet ID long, from ArraySegment<byte>
            var packetId = BitConverter.ToInt64(e.Data.Take(8).ToArray(), 0);
            var packetType = _packetTypes[packetId];
            // get the rest of the data in bytes
            var packetData = e.Data.Skip(8).ToArray();
            // deserialize the packet
            var packet = _serializer.Deserialize(packetType, packetData);
            HandlePacket?.Invoke(packet as IPacket);
        }

        public void SendPacket(IPacket packet)
        {
            var packetId = _packetTypes.Where(x => x.Value == packet.GetType()).FirstOrDefault().Key;
            using var ms = new MemoryStream();
            // write packet ID as bytes
            ms.Write(BitConverter.GetBytes(packetId));
            // serialize packet to bytes
            ms.Write(_serializer.Serialize(packet));

            _client.Send(ms.ToArray());
        }
    }
}
