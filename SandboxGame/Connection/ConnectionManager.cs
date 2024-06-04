using Microsoft.Extensions.DependencyInjection;
using SandboxGame.Api.Entity;
using SandboxGame.Entities;
using SandboxGame.Server.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Connection
{
    public class ConnectionManager
    {
        public IConnectionHandler ConnectionHandler { get; private set; }

        private IServiceProvider _services;

        public ConnectionManager(IConnectionHandler connectionHandler, IServiceProvider services)
        {
            // This is to ensure this class gets initialized semi-early, and no cross-dependency issues occur.
            _services = services;

            ConnectionHandler = connectionHandler;

            connectionHandler.HandlePacket += handlePacket;
            connectionHandler.ConnectionEstablished += () =>
            {
                Console.WriteLine("Connection established!");
                connectionHandler.SendPacket(new ClientInfoPacket() { Username = "Local User" });
            };

            connectionHandler.Connect("127.0.0.1", 42069); // for now, not important how I'll select server.
        }

        private void handlePacket(IPacket packet)
        {
            if(packet is ChatDataPacket chat)
            {
                var chatbox = _services.GetService<IEntityManager>().FindEntityOfType<ChatBox>().FirstOrDefault();
                if (chatbox == null)
                {
                    return;
                }
                chatbox.PushChatMessage(chat.Message, chat.Color);
            }
        }

        public void SendChat(string chat)
        {
            ConnectionHandler.SendPacket(new ChatDataPacket() { Message = chat });
        }
    }
}
