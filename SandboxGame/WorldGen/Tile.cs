using ProtoBuf;

namespace SandboxGame.WorldGen
{
    [ProtoContract]
    public struct Tile
    {
        [ProtoMember(1)]
        public TileType TileType { get; set; }

        public Tile(TileType tileType)
        {
            TileType = tileType;
        }
    }
}
