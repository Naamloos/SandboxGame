using Microsoft.Xna.Framework;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine;
using ProtoBuf;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Cameras;
using SandboxGame.Engine.Storage;

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

        private LoadedSprite grass;
        private LoadedSprite water;
        private LoadedSprite sand;
        private LoadedSprite tree;

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
            this.grass = assetManager.GetSprite("grass");
            this.water = assetManager.GetSprite("water");
            this.sand = assetManager.GetSprite("sand");
            this.tree = assetManager.GetSprite("tree");
            this.tileSize = tileSize;
        }

        public void Update(GameTime gameTime)
        {
            grass.Update(gameTime);
            water.Update(gameTime);
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

                switch(current.TileType)
                {
                    default:
                    case TileType.Grass:
                        grass.Draw(spriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, mirrored, camera, Color.White, tileSize, tileSize, 0);
                        break;
                    case TileType.Water:
                        water.Draw(spriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, false, camera, Color.White, tileSize, tileSize, 0);
                        break;
                    case TileType.Sand:
                        sand.Draw(spriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, mirrored, camera, Color.White, tileSize, tileSize, 0);
                        break;
                }

                if(current.ContainsTree)
                {
                    tree.Draw(spriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, mirrored, camera, Color.White, tileSize, tileSize, 0);
                }
            }
        }
    }
}