using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common.Packets.TwoWay
{
    /// <summary>
    /// Notifies the other side that it's time to disconnect.
    /// </summary>
    public class DisconnectPacket : BasePacket
    {
        public override long PacketId => 4;

        /// <summary>
        /// Reason why client or user disconnected.
        /// </summary>
        public string DisconnectReason { get; set; } = "Disconnected.";
    }
}
