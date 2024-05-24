﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ProtoBuf;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Cameras;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace SandboxGame.WorldGen
{
    public class World
    {
        private WorldInfo _worldInfo;
        private string _name;

        private FastNoiseLite _noise;
        private Dictionary<(int, int), Chunk> _chunkCache = new Dictionary<(int, int), Chunk>();
        private SemaphoreSlim _chunkLock = new SemaphoreSlim(1);

        private AssetManager assetManager;
        private SpriteBatch spriteBatch;
        private Camera camera;

        private World(string name, WorldInfo worldInfo, bool forceNew, AssetManager assetManager, SpriteBatch spriteBatch, Camera camera)
        {
            this.assetManager = assetManager;
            this.spriteBatch = spriteBatch;
            _name = name;
            _worldInfo = worldInfo;

            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            _noise.SetSeed(worldInfo.Seed);

            if(forceNew)
            {
                // Delete all old chunks
                var worldsDir = Path.Combine(Program.WORLDS_PATH, name);
                if (Directory.Exists(worldsDir))
                {
                    Directory.Delete(worldsDir, true);
                }
                Directory.CreateDirectory(worldsDir);
            }
        }

        private Rectangle visibleChunks = new Rectangle(0, 0, 0, 0);

        internal static float LAND_OFFSET = 0.1f;
        public void Update(GameTime gameTime, Camera camera)
        {
            // determining what chunks are visible
            var viewPort = camera.WorldView;
            var fullChunkSize = _worldInfo.ChunkSize * _worldInfo.TileSize;

            var x = (viewPort.X - (viewPort.X % fullChunkSize)) - fullChunkSize;
            var y = (viewPort.Y - (viewPort.Y % fullChunkSize)) - fullChunkSize;
            var width = (viewPort.Width - (viewPort.Width % fullChunkSize)) + fullChunkSize;
            var height = (viewPort.Height - (viewPort.Height % fullChunkSize)) + fullChunkSize;

            visibleChunks = new Rectangle(x / fullChunkSize, y / fullChunkSize, width / fullChunkSize, height / fullChunkSize);

            // get chunks, update them
            for (int cx = visibleChunks.X; cx < visibleChunks.Width; cx++)
            {
                for (int cy = visibleChunks.Y; cy < visibleChunks.Height; cy++)
                {
                    GetChunk(cx, cy).Update(gameTime);
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            // get chunks, update them
            for (int cx = visibleChunks.X; cx < visibleChunks.Width; cx++)
            {
                for (int cy = visibleChunks.Y; cy < visibleChunks.Height; cy++)
                {
                    GetChunk(cx, cy).Draw(gameTime);
                }
            }
        }

        public static World Load(string name, bool forceNew, AssetManager assetManager, SpriteBatch spriteBatch, Camera camera)
        {
            var path = Path.Combine(Program.WORLDS_PATH, $"{name}.dat");

            if(forceNew && File.Exists(path))
            {
                File.Delete(path);
            }

            WorldInfo worldInfo;
            if (File.Exists(path))
            {
                using var file = File.OpenRead(path);
                worldInfo = Serializer.Deserialize<WorldInfo>(file);
            }
            else
            {
                worldInfo = new WorldInfo();
                using var file = File.Create(path);
                Serializer.Serialize(file, worldInfo);
            }

            var world = new World(name, worldInfo, forceNew, assetManager, spriteBatch, camera);
            return world;
        }

        private Chunk GetChunk(int chunkX, int chunkY)
        {
            // TODO unloading, loading from file, saving data, etc
            if (_chunkCache.ContainsKey((chunkX, chunkY)))
            {
                return _chunkCache[(chunkX, chunkY)];
            }

            _chunkLock.Wait();
            if (Chunk.TryLoadFromFile(_name, chunkX, chunkY, _worldInfo.TileSize, assetManager, out Chunk chunk))
            {
                _chunkCache.Add((chunkX, chunkY), chunk);
                _chunkLock.Release();
                return chunk;
            }

            chunk = GenerateChunk(chunkX, chunkY);
            _chunkCache.Add((chunkX, chunkY), chunk);
            _chunkLock.Release();
            return chunk;
        }

        private Chunk GenerateChunk(int chunkX, int chunkY)
        {
            int tileCount = _worldInfo.ChunkSize * _worldInfo.ChunkSize;
            Tile[] tiles = new Tile[tileCount];

            for (int i = 0; i < tileCount; i++)
            {
                int y = (i % _worldInfo.ChunkSize);
                int x = (i - y) / _worldInfo.ChunkSize;

                int worldX = (chunkX * _worldInfo.ChunkSize) + x;
                int worldY = (chunkY * _worldInfo.ChunkSize) + y;

                tiles[i] = new Tile(GenerateTile(worldX, worldY), x == 5 && y == 8);
            }

            var chunk = new Chunk(chunkX, chunkY, _worldInfo.ChunkSize, _worldInfo.TileSize, tiles, assetManager, spriteBatch, camera);
            chunk.SaveToFile(_name);
            return chunk;
        }

        private TileType GenerateTile(int worldX, int worldY)
        {
            float noiseValue = _noise.GetNoise(worldX, worldY);

            if (noiseValue > LAND_OFFSET)
            {
                if (CheckIfTileTouchesWater(worldX, worldY))
                    return TileType.Sand;

                return TileType.Grass;
            }

            return TileType.Water;
        }

        private bool CheckIfTileTouchesWater(int worldX, int worldY)
        {
            return (_noise.GetNoise(worldX - 1, worldY) < LAND_OFFSET)
                || (_noise.GetNoise(worldX, worldY - 1) < LAND_OFFSET)
                || (_noise.GetNoise(worldX + 1, worldY) < LAND_OFFSET)
                || (_noise.GetNoise(worldX, worldY + 1) < LAND_OFFSET)
                || (_noise.GetNoise(worldX + 1, worldY + 1) < LAND_OFFSET)
                || (_noise.GetNoise(worldX - 1, worldY + 1) < LAND_OFFSET)
                || (_noise.GetNoise(worldX + 1, worldY - 1) < LAND_OFFSET)
                || (_noise.GetNoise(worldX - 1, worldY - 1) < LAND_OFFSET);
        }
    }
}
