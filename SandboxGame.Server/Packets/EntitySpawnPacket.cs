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
        public string EntityId { get; set; } = "";

        [ProtoMember(2)]
        public string EntityType { get; set; } = "";

        [ProtoMember(3)]
        public bool ControlledByThisClient { get; set; } = false;

        [ProtoMember(4)]
        public float X { get; set; } = 0f;

        [ProtoMember(5)]
        public float Y { get; set; } = 0f;

        // TODO at some point we'll send more entity data
    }
}
