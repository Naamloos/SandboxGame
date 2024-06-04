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
    [Packet(6, "entity_move")]
    public class EntityMovePacket : IPacket
    {
        [ProtoMember(1)]
        public string EntityId { get; set; }

        [ProtoMember(2)]
        public float X { get; set; }

        [ProtoMember(3)]
        public float Y { get; set; }
    }
}
