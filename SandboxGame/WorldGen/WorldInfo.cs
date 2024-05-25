using ProtoBuf;
using System;

namespace SandboxGame.WorldGen
{
    [ProtoContract]
    public class WorldInfo
    {
        [ProtoMember(1)]
        public string Description { get; set; } = "My World!";

        [ProtoMember(2)]
        public int Seed { get; set; } = Random.Shared.Next();

        [ProtoMember(3)]
        public int ChunkSize { get; set; } = 96;

        [ProtoMember(4)]
        public int TileSize { get; set; } = 32;

        public WorldInfo() { }
    }
}
