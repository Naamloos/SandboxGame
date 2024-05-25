using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxGame.Common.Packets.TwoWay;
using SandboxGame.Common.Packets.ServerBound;

namespace SandboxGame.Common.Packets.Clientbound
{
    /// <summary>
    /// Packet sent by server to inform the client of it's info. If a client sends a <see cref="DisconnectPacket"/> right after, it's a server list ping.
    /// If a <see cref="ClientInfoPacket"/> is sent right after, the client is connecting.
    /// </summary>
    public class ServerInfoPacket : BasePacket
    {
        public override long PacketId => 0;

        /// <summary>
        /// Protocol version this server supports
        /// </summary>
        public int ProtocolVersion { get; set; } = 0;

        /// <summary>
        /// Name of this server
        /// </summary>
        public string ServerName { get; set; } = "SandboxGame Server";

        /// <summary>
        /// Max amount of players that can connect to this server.
        /// </summary>
        public int MaxPlayers { get; set; } = 25;

        /// <summary>
        /// Currently online players
        /// </summary>
        public int CurrentPlayers { get; set; } = 0;

        /// <summary>
        /// Icon Base64 hash for this server
        /// </summary>
        public string IconB64 { get; set; } = ""; // TODO default icon? maybe generated in-code from a resource

        /// <summary>
        /// Message of the day
        /// </summary>
        public string Motd { get; set; } = "Welcome to my server!";
    }
}
