using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server
{
    public class RemoteClient
    {
        public string GameClientIdentifier { get; private set; }
        public string ConnectionIdentifier { get; private set; }
        public ClientInfoPacket ClientInfo { get; private set; }
        public ulong PlayerEntityId { get; set; } = 0;

        private GameServer _server;

        public RemoteClient(string gameClientIdentifier, string connectionIdentifier, ClientInfoPacket client, GameServer server)
        {
            GameClientIdentifier = gameClientIdentifier;
            ConnectionIdentifier = connectionIdentifier;
            ClientInfo = client;
            _server = server;
        }

        public void AssignPlayerEntity(ulong entityId)
        {
            PlayerEntityId = entityId;
        }

        public void SendPacket(IPacket packet)
        {
            _server.SendPacketToClient(ConnectionIdentifier, packet);
        }

        public void HandleIncomingPacket(IPacket packet)
        {
            if(packet is ChatDataPacket chat)
            {
                HandleChatData(chat);
                return;
            }
        }

        private void HandleChatData(ChatDataPacket chat)
        {
            if(chat.Message.StartsWith('/'))
            {
                _server.SendPacketToClient(ConnectionIdentifier, new ChatDataPacket 
                { 
                    Message = "Commands are not yet implemented.",
                    Color = 0xFF0000FF // Red
                });
                return;
            }
            Console.WriteLine($"[{ClientInfo.Username}] {chat.Message}");
            _server.SendPacketToAllClients(new ChatDataPacket { Message = $"[{ClientInfo.Username}] {chat.Message}" });
        }
    }
}
