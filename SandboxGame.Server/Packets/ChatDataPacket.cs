using ProtoBuf;
using SandboxGame.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Packets
{
    [ProtoContract]
    [Packet(2, "chat_data")]
    public class ChatDataPacket : IPacket
    {
        /// <summary>
        /// Chat message data. When client-bound, this will be displayed in chat as-is. 
        /// When server-bound, this could also be interpreted as a command, and possible a username could be prepended.
        /// </summary>
        [ProtoMember(1)]
        public string Message { get; set; } = "";

        /// <summary>
        /// Represents the color for this chat message. Gets ignored by the server.
        /// </summary>
        [ProtoMember(2)]
        public uint Color { get; set; } = 0xFFFFFFFF;
    }
}
