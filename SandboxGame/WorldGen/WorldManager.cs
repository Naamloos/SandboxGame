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

namespace SandboxGame.WorldGen
{
    public class WorldManager
    {
        private FastNoiseLite _noise;
        private int _chunkSize;
        private int _tileSize;

        private GameContext _gameContext;
        private Dictionary<(int, int), Chunk> _chunkCache = new Dictionary<(int, int), Chunk>();
        private SemaphoreSlim _chunkLock = new SemaphoreSlim(1);

        public WorldManager(GameContext gameContext, int chunkSize = 64, int tileSize = 64) 
        {
            _noise = new FastNoiseLite();
            _noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            _noise.SetSeed(Random.Shared.Next());
            _chunkSize = chunkSize;
            _tileSize = tileSize;
            _gameContext = gameContext;
        }

        private Rectangle visibleChunks = new Rectangle(0, 0, 0, 0);

        private bool flip = false;
        internal static float LAND_OFFSET = 0.5f;
        public void Update(GameTime gameTime)
        {
            //if (flip)
            //{
            //    LAND_OFFSET += 0.05f;
            //}
            //else
            //{
            //    LAND_OFFSET -= 0.05f;
            //}

            //if (LAND_OFFSET < -1.0f)
            //    flip = true;
            //if (LAND_OFFSET > 1.0f)
            //    flip = false;

            // determining what chunks are visible
            var viewPort = _gameContext.Camera.WorldView;
            var fullChunkSize = _chunkSize * _tileSize;

            var x = (viewPort.X - (viewPort.X % fullChunkSize)) - fullChunkSize;
            var y = (viewPort.Y - (viewPort.Y % fullChunkSize)) - fullChunkSize;
            var width = (viewPort.Width - (viewPort.Width % fullChunkSize)) + fullChunkSize;
            var height = (viewPort.Height - (viewPort.Height % fullChunkSize)) + fullChunkSize;

            visibleChunks = new Rectangle(x / fullChunkSize, y / fullChunkSize, width / fullChunkSize, height / fullChunkSize);
            DebugHelper.SetDebugValues("WORLDGEN", $"start: x{visibleChunks.X} y{visibleChunks.Y} amount: x{visibleChunks.Width} y{visibleChunks.Height}");

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

        public bool IsWalkable(int worldX, int worldY)
        {
            // TODO check in chunk
            return true;
        }

        private Chunk GetChunk(int chunkX, int chunkY)
        {
            // TODO unloading, loading from file, saving data, etc
            if(_chunkCache.ContainsKey((chunkX, chunkY)))
            {
                return _chunkCache[(chunkX, chunkY)];
            }

            _chunkLock.Wait();
            int startX = 0;
            int startY = 0;
            int endX = _chunkSize;
            int endY = _chunkSize;

            float[][] tiles = new float[_chunkSize][];

            for(int x = startX; x < endX; x++)
            {
                tiles[x] = new float[_chunkSize];
                for(int y = startY; y < endY; y++)
                {
                    tiles[x][y] = _noise.GetNoise((chunkX * _chunkSize) + x, (chunkY * _chunkSize) + y);
                }
            }

            var chunk = new Chunk(chunkX, chunkY, tiles, _gameContext, _tileSize);
            _chunkCache.Add((chunkX, chunkY), chunk);
            _chunkLock.Release();
            return chunk;
        }
    }

    public class Chunk
    {
        private float[][] tiles;
        private Sprite grass;
        private Sprite water;

        private int chunkX;
        private int chunkY;
        private int _tileSize;

        private SpriteBatch spriteBatch;
        private GameContext ctx;

        public Chunk(int chunkX, int chunkY, float[][] tiles, GameContext gameContext, int tileSize)
        {
            this.tiles = tiles;
            this.grass = gameContext.AssetManager.GetSprite("grass");
            this.water = gameContext.AssetManager.GetSprite("water");

            this.chunkX = chunkX;
            this.chunkY = chunkY;
            _tileSize = tileSize;

            spriteBatch = gameContext.SpriteBatch;
            ctx = gameContext;
        }

        public void Update(GameTime gameTime)
        {
            DebugHelper.SetDebugValues("LAND_OFFSET", WorldManager.LAND_OFFSET.ToString() + "f");

            grass.Update(gameTime);
            water.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            int startX = (chunkX * _tileSize) * tiles.Length;

            for (int x = 0; x < tiles.Length; x++)
            {
                int startY = (chunkY * _tileSize) * tiles.Length;
                for(int y = 0; y < tiles[x].Length; y++)
                {
                    if (tiles[x][y] > WorldManager.LAND_OFFSET)
                        grass.Draw(spriteBatch, startX + (x * _tileSize), startY + (y * _tileSize), widthOverride: _tileSize, heightOverride: _tileSize);
                    else
                        water.Draw(spriteBatch, startX + (x * _tileSize), startY + (y * _tileSize), widthOverride: _tileSize, heightOverride: _tileSize);
                }
            }
        }
    }
}
