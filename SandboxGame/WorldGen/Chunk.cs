using Microsoft.Xna.Framework;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine;
using ProtoBuf;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Storage;
using SandboxGame.Api;
using SandboxGame.Api.Assets;
using SandboxGame.Api.Units;

namespace SandboxGame.WorldGen
{
    [ProtoContract]
    public class Chunk
    {
        [ProtoMember(1)]
        public int FileVersion { get; set; } = 1;

        [ProtoMember(2)]
        public int ChunkX { get; set; }

        [ProtoMember(3)]
        public int ChunkY { get; set; }

        [ProtoMember(4)]
        public int ChunkSize { get; set; } = 32;

        [ProtoMember(5)]
        public Tile[] Tiles { get; set; }

        private ILoadedSprite grass;
        private ILoadedSprite water;
        private ILoadedSprite sand;
        private ILoadedSprite tree;

        private int tileSize;

        private SpriteBatch spriteBatch;
        private Camera camera;

        public Chunk(int chunkX, int chunkY, int chunkSize, int tileSize, Tile[] tiles, AssetManager assetManager, 
            SpriteBatch spriteBatch, Camera camera)
        {
            this.camera = camera;
            this.Tiles = tiles;
            this.ChunkX = chunkX;
            this.ChunkY = chunkY;
            this.ChunkSize = chunkSize;

            Initialize(tileSize, assetManager, spriteBatch);
        }

        // for serialization
        private Chunk() { }

        public void Initialize(int tileSize, AssetManager assetManager, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.grass = assetManager.GetSprite("nature_grass");
            this.water = assetManager.GetSprite("nature_water");
            this.sand = assetManager.GetSprite("nature_sand");
            this.tree = assetManager.GetSprite("nature_bush");
            this.tileSize = tileSize;
        }

        public void Update(GameTime gameTime)
        {
            grass.Update();
            water.Update();
        }

        public void Draw(GameTime gameTime)
        {
            if(Tiles == null || Tiles.Length == 0) return;

            int startX = (ChunkX * tileSize) * ChunkSize;
            int startY = (ChunkY * tileSize) * ChunkSize;

            for(int i = 0; i < this.Tiles.Length; i++)
            {
                int y = (i % ChunkSize);
                int x = (i - y) / ChunkSize;
                Tile current = this.Tiles[i];
                bool mirrored = x % 3 + y % 6 == 0;

                var dest = new RectangleUnit(startX + (x * tileSize), startY + (y * tileSize), tileSize, tileSize);
                switch(current.TileType)
                {
                    default:
                    case TileType.Grass:
                        grass.Draw(dest, false, mirrored, ColorHelper.RGBA(255,255,255), 0);
                        break;
                    case TileType.Water:
                        water.Draw(dest, false, false, ColorHelper.RGBA(255, 255, 255), 0);
                        break;
                    case TileType.Sand:
                        sand.Draw(dest, false, mirrored, ColorHelper.RGBA(255, 255, 255), 0);
                        break;
                }

                if(current.ContainsTree)
                {
                    tree.Draw(dest, false, mirrored, ColorHelper.RGBA(255, 255, 255), 0);
                }
            }
        }
    }
}