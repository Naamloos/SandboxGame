using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxGame.Engine;
using SandboxGame.Engine.Cameras;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Entities
{
    public abstract class BaseEntity : ICameraTarget
    {
        protected GameContext GameContext { get; private set; }

        public BaseEntity(GameContext gameContext)
        {
            this.GameContext = gameContext;
        }

        public abstract Rectangle Bounds { get; }

        public abstract Vector2 Position { get; set; }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
