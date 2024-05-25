using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common.Packets.TwoWay
{
    /// <summary>
    /// Packet for mods to communicate custom data if needed.
    /// </summary>
    public class ModCustomPacket : BasePacket
    {
        public override long PacketId => 2;

        /// <summary>
        /// Used by mods to identify that this packet belongs to them
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// For mods to identify the type of packet before deserializing data
        /// </summary>
        public string PacketKey { get; set; }

        /// <summary>
        /// Binary packet data (de)serialized by modded packets
        /// </summary>
        public byte[] Data { get; set; }
    }
}
