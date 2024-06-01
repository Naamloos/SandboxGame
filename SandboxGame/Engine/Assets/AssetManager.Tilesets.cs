using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Assets
{
    public partial class AssetManager
    {
        public TilesetMap GetNatureTilemap() => new TilesetMap()
        {
            Name = "nature",
            TileHeight = 32,
            TileWidth = 32,
            Tiles = new MappedTile[]
            {
                new MappedTile() { Index = 0, Name = "tree_top" },
                new MappedTile() { Index = 1, Name = "tree_bottom" },
                new MappedTile() { Index = 2, Name = "grass" },
                new MappedTile() { Index = 10, Name = "bush" },
                new MappedTile() { Index = 9, Name = "tree_overlap" },
                new MappedTile() { Index = 11, Name = "water" },
                new MappedTile() { Index = 18, Name = "sign" },
                new MappedTile() { Index = 19, Name = "rock" },
                new MappedTile() { Index = 20, Name = "sand" },
            }
        };
    }
}
