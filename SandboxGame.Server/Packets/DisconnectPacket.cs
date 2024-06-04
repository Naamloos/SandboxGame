using ProtoBuf;
using SandboxGame.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Packets
{
    /// <summary>
    /// Notifies the other side that it's time to disconnect.
    /// </summary>
    [ProtoContract]
    [Packet(3, "disconnect")]
    public class DisconnectPacket : IPacket
    {
        /// <summary>
        /// Reason why client or user disconnected.
        /// </summary>
        [ProtoMember(1)]
        public string DisconnectReason { get; set; } = "Disconnected.";
    }
}
