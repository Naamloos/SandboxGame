using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SandboxGame.World
{
    public class WorldManager
    {
        private const int CHUNK_SIZE = 96;
        private const int TILE_SIZE = 32;

        private FastNoiseLite _noise;

        private GameContext _gameContext;
        private Dictionary<(int, int), Chunk> _chunkCache = new Dictionary<(int, int), Chunk>();
        private SemaphoreSlim _chunkLock = new SemaphoreSlim(1);

        public WorldManager(GameContext gameContext) 
        {
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            _noise.SetSeed(Random.Shared.Next());
            _gameContext = gameContext;
        }

        private Rectangle visibleChunks = new Rectangle(0, 0, 0, 0);

        internal static float LAND_OFFSET = 0.5f;
        public void Update(GameTime gameTime)
        {
            // determining what chunks are visible
            var viewPort = _gameContext.Camera.WorldView;
            var fullChunkSize = CHUNK_SIZE * TILE_SIZE;

            var x = (viewPort.X - (viewPort.X % fullChunkSize)) - fullChunkSize;
            var y = (viewPort.Y - (viewPort.Y % fullChunkSize)) - fullChunkSize;
            var width = (viewPort.Width - (viewPort.Width % fullChunkSize)) + fullChunkSize;
            var height = (viewPort.Height - (viewPort.Height % fullChunkSize)) + fullChunkSize;

            visibleChunks = new Rectangle(x / fullChunkSize, y / fullChunkSize, width / fullChunkSize, height / fullChunkSize);

            // get chunks, update them
            for(int cx = visibleChunks.X; cx < visibleChunks.Width; cx++)
            {
                for(int cy = visibleChunks.Y; cy < visibleChunks.Height; cy++)
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

        private Chunk GetChunk(int chunkX, int chunkY)
        {
            // TODO unloading, loading from file, saving data, etc
            if(_chunkCache.ContainsKey((chunkX, chunkY)))
            {
                return _chunkCache[(chunkX, chunkY)];
            }

            _chunkLock.Wait();
            if (Chunk.TryLoadFromFile(chunkX, chunkY, _gameContext, TILE_SIZE, out Chunk chunk))
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
            int tileCount = CHUNK_SIZE * CHUNK_SIZE;
            Tile[] tiles = new Tile[tileCount];

            for(int i = 0; i < tileCount; i++)
            {
                int y = (i % CHUNK_SIZE);
                int x = (i - y) / CHUNK_SIZE;

                int worldX = (chunkX * CHUNK_SIZE) + x;
                int worldY = (chunkY * CHUNK_SIZE) + y;

                tiles[i] = new Tile(GenerateTile(worldX, worldY));
            }

            var chunk = new Chunk(chunkX, chunkY, CHUNK_SIZE, TILE_SIZE, tiles, _gameContext);
            chunk.SaveToFile();
            return chunk;
        }

        private TileType GenerateTile(int worldX, int worldY)
        {
            float noiseValue = _noise.GetNoise(worldX, worldY);

            if(noiseValue > LAND_OFFSET)
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
                || (_noise.GetNoise(worldX, worldY + 1) < LAND_OFFSET);
        }
    }
}
