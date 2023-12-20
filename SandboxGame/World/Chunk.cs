﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.IO;
using System.Reflection;

namespace SandboxGame.World
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

        private GameContext ctx;
        private int tileSize;

        public Chunk(int chunkX, int chunkY, int chunkSize, int tileSize, Tile[] tiles, GameContext gameContext)
        {
            this.Tiles = tiles;
            this.ChunkX = chunkX;
            this.ChunkY = chunkY;
            this.ChunkSize = chunkSize;

            initialize(gameContext, tileSize);
        }

        // for serialization
        private Chunk() { }

        private void initialize(GameContext ctx, int tileSize)
        {
            this.ctx = ctx;
            this.grass = ctx.AssetManager.GetSprite("grass");
            this.water = ctx.AssetManager.GetSprite("water");
            this.sand = ctx.AssetManager.GetSprite("sand");
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
                        grass.Draw(ctx.SpriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, mirrored, ctx.Camera, Color.White, tileSize, tileSize, 0);
                        break;
                    case TileType.Water:
                        water.Draw(ctx.SpriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, false, ctx.Camera, Color.White, tileSize, tileSize, 0);
                        break;
                    case TileType.Sand:
                        sand.Draw(ctx.SpriteBatch, startX + (x * tileSize), startY + (y * tileSize), false, mirrored, ctx.Camera, Color.White, tileSize, tileSize, 0);
                        break;
                }
            }
        }

        private static string basePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "chunks");

        public static bool TryLoadFromFile(int x, int y, GameContext ctx, int tileSize, out Chunk chunk)
        {
            var filePath = Path.Combine(basePath, $"{x}-{y}.bin");
            if(!File.Exists(filePath))
            {
                chunk = null;
                return false;
            }

            using var file = File.OpenRead(filePath);
            
            chunk = Serializer.Deserialize<Chunk>(file);
            chunk.initialize(ctx, tileSize);

            return true;
        }

        public void SaveToFile()
        {
            if(!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            var filePath = Path.Combine(basePath, $"{ChunkX}-{ChunkY}.bin");

            bool exists = File.Exists(filePath);

            using var file = exists? File.OpenWrite(filePath) : File.Create(filePath);
            Serializer.Serialize(file, this);
        }
    }
}