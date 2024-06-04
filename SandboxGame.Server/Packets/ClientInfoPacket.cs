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
    /// Followed by the server's <see cref="ServerInfoPacket"/> to start connecting.
    /// </summary>
    [ProtoContract]
    [Packet(1, "client_info")]
    public class ClientInfoPacket : IPacket
    {
        /// <summary>
        /// Username configured by client.
        /// </summary>
        [ProtoMember(1)]
        public string Username { get; set; } = "Anonymous#" + Random.Shared.Next(0, 9999).ToString().PadLeft(4, '0');

        /// <summary>
        /// Protocol version this client supports
        /// </summary>
        [ProtoMember(2)]
        public int ProtocolVersion { get; set; } = 0;

        /// <summary>
        /// If any mods are missing here, 
        /// the server can reply with a MissingContentPacket to notify the client it can't accept their connection.
        /// </summary>
        [ProtoMember(3)]
        public string[] ModHashesMD5 { get; set; } = new string[0]; // Not Modded by default :3 
    }
}
