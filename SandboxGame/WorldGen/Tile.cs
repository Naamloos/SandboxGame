using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
