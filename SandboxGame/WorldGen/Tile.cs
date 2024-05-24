using ProtoBuf;

namespace SandboxGame.WorldGen
{
    [ProtoContract]
    public struct Tile
    {
        [ProtoMember(1)]
        public TileType TileType { get; set; }

        [ProtoMember(2)]
        public bool ContainsTree { get; set; }

        public Tile(TileType tileType, bool containsTree = false)
        {
            TileType = tileType;
            ContainsTree = containsTree;
        }
    }
}
