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
        public string Identifier { get; private set; }
        public ClientInfoPacket ClientInfo { get; private set; }

        private GameServer _server;

        public RemoteClient(string identifier, ClientInfoPacket client, GameServer server)
        {
            Identifier = identifier;
            ClientInfo = client;
            _server = server;
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
                _server.SendPacketToClient(Identifier, new ChatDataPacket 
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
