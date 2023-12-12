using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Engine
{
    public abstract class BaseScene : IDisposable
    {
        public abstract void Initialize(GameContext gameContext);

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime);

        public abstract void DrawUI(GameTime gameTime);

        public abstract void Dispose();
    }
}
