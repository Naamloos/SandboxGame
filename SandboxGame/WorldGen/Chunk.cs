using Microsoft.Xna.Framework;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine;
using ProtoBuf;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine.Cameras;

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

        private Sprite grass;
        private Sprite water;
        private Sprite sand;
        private Sprite tree;

        private int tileSize;

        private SpriteBatch spriteBatch;
        private Camera camera;

        public Chunk(int chunkX, int chunkY, int chunkSize, int tileSize, Tile[] tiles, AssetManager assetManager, SpriteBatch spriteBatch, Camera camera)
        {
            this.spriteBatch = spriteBatch;
            this.camera = camera;
            this.Tiles = tiles;
            this.ChunkX = chunkX;
            this.ChunkY = chunkY;
            this.ChunkSize = chunkSize;

            initialize(tileSize, assetManager);
        }

        // for serialization
        private Chunk() { }

        private void initialize(int tileSize, AssetManager assetManager)
        {
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

        public static bool TryLoadFromFile(string worldName, int x, int y, int tileSize, AssetManager assetManager, out Chunk chunk)
        {
            var filePath = Path.Combine(Program.WORLDS_PATH, worldName, $"{x}-{y}.bin");
            if(!File.Exists(filePath))
            {
                chunk = null;
                return false;
            }

            using var file = File.OpenRead(filePath);
            
            chunk = Serializer.Deserialize<Chunk>(file);
            chunk.initialize(tileSize, assetManager);

            return true;
        }

        public void SaveToFile(string worldName)
        {
            if(!Directory.Exists(Path.Combine(Program.WORLDS_PATH, worldName)))
            {
                Directory.CreateDirectory(Path.Combine(Program.WORLDS_PATH, worldName));
            }

            var filePath = Path.Combine(Program.WORLDS_PATH, worldName, $"{ChunkX}-{ChunkY}.bin");

            bool exists = File.Exists(filePath);

            using var file = exists? File.OpenWrite(filePath) : File.Create(filePath);
            Serializer.Serialize(file, this);
        }
    }
}