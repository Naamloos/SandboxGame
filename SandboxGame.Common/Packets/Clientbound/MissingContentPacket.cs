using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Common.Packets.Clientbound
{
    /// <summary>
    /// Notifies the client of what mods it's missing. Excludes MD5 hash to prevent clients with missing mods from spoofing this info too easily.
    /// </summary>
    public class MissingContentPacket : BasePacket
    {
        public override long PacketId => 3;

        /// <summary>
        /// Name of the missing mod
        /// </summary>
        public string ModName { get; set; } = "Mod";

        /// <summary>
        /// Version of the missing mod
        /// </summary>
        public string ModVersionString { get; set; } = "1.0";
    }
}
