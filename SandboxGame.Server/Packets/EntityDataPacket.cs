using ProtoBuf;
using SandboxGame.Api.Entity;
using SandboxGame.Api.Units;
using SandboxGame.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Server.Packets
{
    [ProtoContract]
    [Packet(6, "entity_data")]
    public class EntityDataPacket : IPacket
    {
        [ProtoMember(1)]
        public ulong EntityId { get; set; }

        [ProtoMember(2)]
        public float X { get; set; }

        [ProtoMember(3)]
        public float Y { get; set; }

        [ProtoMember(4)]
        public float Width { get; set; }

        [ProtoMember(5)]
        public float Height { get; set; }

        [ProtoMember(6)]
        public bool IsInteractable { get; set; }

        public EntityDataPacket() { }

        public EntityDataPacket(BaseEntity entity)
        {
            EntityId = entity.EntityId;
            X = entity.Bounds.X;
            Y = entity.Bounds.Y;
            Width = entity.Bounds.Width;
            Height = entity.Bounds.Height;
            IsInteractable = entity.IsInteractable;
        }

        internal void Apply(BaseEntity entity)
        {
            entity._bounds = new RectangleUnit(X, Y, Width, Height);
            entity._isInteractable = IsInteractable;
        }
    }
}
