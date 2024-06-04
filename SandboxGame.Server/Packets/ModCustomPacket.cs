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
    /// Packet for mods to communicate custom data if needed.
    /// </summary>
    [ProtoContract]
    [Packet(5, "mod_custom_packet")]
    public class ModCustomPacket : IPacket
    {
        /// <summary>
        /// Used by mods to identify that this packet belongs to them.
        /// It is recommended to use your mod's Assembly namespace.
        /// </summary>
        [ProtoMember(1)]
        public string Namespace { get; set; } = "";

        /// <summary>
        /// For mods to identify the type of packet before deserializing data
        /// </summary>
        [ProtoMember(2)]
        public string PacketKey { get; set; } = "";

        /// <summary>
        /// Binary packet data (de)serialized by modded packets
        /// </summary>
        [ProtoMember(3)]
        public byte[] Data { get; set; } = new byte[0];
    }
}
