using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api.Units;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine.Assets
{
    public class TilesetParser
    {
        private Texture2D _tileSet;
        private TilesetMap _map;

        public TilesetParser(Texture2D tileSet, TilesetMap map)
        {
            this._tileSet = tileSet;
            this._map = map;
        }

        public Dictionary<string, LoadedTile> Load(Effect colorOverlay, SpriteBatch spriteBatch, GameTimeHelper gameTimeHelper, MouseHelper mouseHelper)
        {
            var tiles = new Dictionary<string, LoadedTile>();
            foreach (var tile in _map.Tiles)
            {
                var tilesetWidth = _tileSet.Width; // 32
                var tilesetHeight = _tileSet.Height; // 32

                // rock would be on y 1, x 2
                // its index is 19

                var xTiles = tilesetWidth / _map.TileWidth; // 20
                var yTiles = tilesetHeight / _map.TileHeight; // 9

                // use the tile index to determine the x and y position of the tile, and make sure to wrap around the tileset
                int y = tile.Index % yTiles; // 1
                // now use these aforementioned values to calculate x
                int xRemainder = tile.Index - y; // 18
                int x = xRemainder / yTiles; // 2

                var tilePosition = new PointUnit(x * _map.TileWidth, y * _map.TileHeight); // Load tile
                // Load tile
                tiles.Add(_map.Name + "_" + tile.Name, 
                    new LoadedTile(_map.TileWidth, _map.TileHeight, colorOverlay, spriteBatch, gameTimeHelper, mouseHelper, _tileSet, tilePosition));
            }

            return tiles;
        }
    }
}
