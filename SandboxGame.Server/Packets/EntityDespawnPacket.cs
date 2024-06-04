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
    [Packet(7, "entity_despawn")]
    public class EntityDespawnPacket : IPacket
    {
        [ProtoMember(1)]
        public string EntityId { get; set; } = "";
    }
}
