using ProtoBuf;
using SandboxGame.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Packets
{
    [ProtoContract]
    [Packet(9, "set_entity_owner")]
    public class SetEntityOwnerPacket : IPacket
    {
        [ProtoMember(1)]
        public ulong EntityId { get; set; } = 0;

        [ProtoMember(2)]
        public string Owner { get; set; } = "";
    }
}
