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
    /// Notifies the client of what mods it's missing. Excludes MD5 hash to prevent clients with missing mods from spoofing this info too easily.
    /// </summary>
    [ProtoContract]
    [Packet(4, "missing_content")]
    public class MissingContentPacket : IPacket
    {
        /// <summary>
        /// Name of the missing mod
        /// </summary>
        [ProtoMember(1)]
        public string ModName { get; set; } = "Mod";

        /// <summary>
        /// Version of the missing mod
        /// </summary>
        [ProtoMember(2)]
        public string ModVersionString { get; set; } = "1.0";
    }
}
