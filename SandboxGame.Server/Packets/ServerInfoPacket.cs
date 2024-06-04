using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using SandboxGame.Server.Attributes;

namespace SandboxGame.Server.Packets
{
    /// <summary>
    /// Packet sent by server to inform the client of it's info. If a client sends a <see cref="DisconnectPacket"/> right after, it's a server list ping.
    /// If a <see cref="ClientInfoPacket"/> is sent right after, the client is connecting.
    /// </summary>
    [ProtoContract]
    [Packet(0, "server_info")]
    public class ServerInfoPacket : IPacket
    {
        /// <summary>
        /// Protocol version this server supports
        /// </summary>
        [ProtoMember(1)]
        public int ProtocolVersion { get; set; } = 0;

        /// <summary>
        /// Name of this server
        /// </summary>
        [ProtoMember(2)]
        public string ServerName { get; set; } = "SandboxGame Server";

        /// <summary>
        /// Max amount of players that can connect to this server.
        /// </summary>
        [ProtoMember(3)]
        public int MaxPlayers { get; set; } = 25;

        /// <summary>
        /// Currently online players
        /// </summary>
        [ProtoMember(4)]
        public int CurrentPlayers { get; set; } = 0;

        /// <summary>
        /// Icon Base64 hash for this server
        /// </summary>
        [ProtoMember(5)]
        public byte[] IconData { get; set; } = new byte[0]; // TODO default icon? maybe generated in-code from a resource

        /// <summary>
        /// Message of the day
        /// </summary>
        [ProtoMember(6)]
        public string Motd { get; set; } = "Welcome to my server!";
    }
}
