using Microsoft.Xna.Framework;
using SandboxGame.Engine;
using SandboxGame.Engine.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.World
{
    public class WorldInteractionBox
    {
        private Sprite tile;
        private GameContext gameContext;
        private Vector2 position = Vector2.Zero;

        public WorldInteractionBox(GameContext ctx)
        {
            gameContext = ctx;
            tile = ctx.AssetManager.GetSprite("interact");
        }

        public void Update(GameTime gameTime)
        {
            var pos = gameContext.MouseHelper.WorldPos;

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

        public void Draw(GameTime gameTime)
        {
            tile.Draw(gameContext.SpriteBatch, (int)position.X, (int)position.Y, lightColor: new Color(255,255,255,150));
        }
    }
}
