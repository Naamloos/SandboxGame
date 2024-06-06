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
    [Packet(8, "entity_spawn")]
    public class EntitySpawnPacket : IPacket
    {
        [ProtoMember(1)]
        public ulong EntityId { get; set; } = 0;

        [ProtoMember(2)]
        public string EntityType { get; set; } = "";
    }
}
