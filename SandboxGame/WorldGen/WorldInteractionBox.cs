using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Vector2 position = Vector2.Zero;
        private MouseHelper mouseHelper;
        private SpriteBatch spriteBatch;

        public override RectangleUnit Bounds => new RectangleUnit(0,0,0,0);

        public override Vector2 Position { get => position; set => position = value; }

        public override bool IsWorldEntity => true;

        public WorldInteractionBox(MouseHelper mouseHelper, SpriteBatch spriteBatch, AssetManager assetManager)
        {
            this.mouseHelper = mouseHelper;
            this.spriteBatch = spriteBatch;
            tile = assetManager.GetSprite("interact");
        }

        public override void Update(GameTime gameTime)
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

            position = new Vector2(x, y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            tile.Draw(spriteBatch, (int)position.X, (int)position.Y, lightColor: new Color(255, 255, 255, 150));
        }
    }
}
