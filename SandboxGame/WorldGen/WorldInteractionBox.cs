using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Api;
using SandboxGame.Api.Units;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using SandboxGame.Engine.Entity;
using SandboxGame.Engine.Input;
using System;

namespace SandboxGame.WorldGen
{
    public class WorldInteractionBox : BaseEntity
    {
        private LoadedSprite tile;
        private PointUnit position = PointUnit.Zero;
        private MouseHelper mouseHelper;
        private SpriteBatch spriteBatch;

        public override RectangleUnit Bounds => new RectangleUnit(0,0,0,0);

        public override PointUnit Position { get => position; set => position = value; }

        public override bool IsWorldEntity => true;

        public override bool Interactable => false;

        public WorldInteractionBox(MouseHelper mouseHelper, SpriteBatch spriteBatch, AssetManager assetManager)
        {
            this.mouseHelper = mouseHelper;
            this.spriteBatch = spriteBatch;
            tile = assetManager.GetSprite("interact");
        }

        public override void Update()
        {
            var pos = mouseHelper.WorldPos;

            // This could be better but it works.
            bool xNegative = pos.X < 0;
            bool yNegative = pos.Y < 0;

            int x = (int)Math.Abs(Math.Floor(pos.X));
            x = x - (x % 32);
            x = xNegative ? (-x - 32) : x;

            int y = (int)Math.Abs(Math.Floor(pos.Y));
            y = y - (y % 32);
            y = yNegative ? (-y - 32) : y;

            position = new PointUnit(x, y);
        }

        public override void Draw()
        {
            tile.Draw((int)position.X, (int)position.Y, lightColor: ColorHelper.RGBA(255, 255, 255, 150));
        }
    }
}
