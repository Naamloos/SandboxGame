using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common.Packets
{
    /// <summary>
    /// The basis for all server/client packets
    /// </summary>
    public abstract class BasePacket
    {
        /// <summary>
        /// ID of this packet.
        /// </summary>
        public abstract long PacketId { get; }
    }
}
