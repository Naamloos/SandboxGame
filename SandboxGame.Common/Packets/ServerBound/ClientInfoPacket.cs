using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandboxGame.Common.Packets.Clientbound;

namespace SandboxGame.Common.Packets.ServerBound
{
    /// <summary>
    /// Followed by the server's <see cref="ServerInfoPacket"/> to start connecting.
    /// </summary>
    public class ClientInfoPacket : BasePacket
    {
        public override long PacketId => 1;

        /// <summary>
        /// Username configured by client.
        /// </summary>
        public string Username { get; set; } = "Anonymous#" + Random.Shared.Next(0, 9999).ToString().PadLeft(4, '0');

        /// <summary>
        /// Protocol version this client supports
        /// </summary>
        public int ProtocolVersion { get; set; } = 0;

        /// <summary>
        /// If any mods are missing here, 
        /// the server can reply with a MissingContentPacket to notify the client it can't accept their connection.
        /// </summary>
        public string[] ModHashesMD5 { get; set; } = new string[0]; // Not Modded by default :3 
    }
}
