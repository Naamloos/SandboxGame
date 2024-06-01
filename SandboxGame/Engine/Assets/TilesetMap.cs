using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Assets
{
    public struct TilesetMap
    {
        public string Name { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public MappedTile[] Tiles { get; set; }
    }

    public struct MappedTile
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }
}
